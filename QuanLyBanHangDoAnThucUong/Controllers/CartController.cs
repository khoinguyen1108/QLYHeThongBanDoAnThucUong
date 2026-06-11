using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models.Entities;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentCustomerId()
        {
            var maKH = HttpContext.Session.GetInt32("MaKH");
            if (maKH != null)
                return maKH;

            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            if (maTaiKhoan != null && vaiTro == "KhachHang")
            {
                var khachHang = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == maTaiKhoan);
                if (khachHang != null)
                {
                    HttpContext.Session.SetInt32("MaKH", khachHang.MaKH);
                    return khachHang.MaKH;
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return Json(new { count = 0 });

            var gioHang = await _context.GioHangs
                .Include(g => g.CT_GioHangs)
                .FirstOrDefaultAsync(g => g.MaKH == maKH);

            if (gioHang == null) return Json(new { count = 0 });

            var totalItems = gioHang.CT_GioHangs.Sum(c => c.SoLuong);
            return Json(new { count = totalItems });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int maBienThe, int soLuong, bool forceClear = false)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null)
            {
                return Json(new { success = false, requireLogin = true, message = "Vui lòng đăng nhập để thêm vào giỏ hàng." });
            }

            var bienThe = await _context.BienTheMonAns
                .Include(b => b.MonAn)
                .FirstOrDefaultAsync(b => b.MaBienThe == maBienThe);

            if (bienThe == null || bienThe.MonAn == null)
            {
                return Json(new { success = false, message = "Món ăn không tồn tại." });
            }

            var maGianHangMoi = bienThe.MonAn.MaGianHang;

            var gioHang = await _context.GioHangs
                .Include(g => g.CT_GioHangs)
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(b => b.MonAn)
                .FirstOrDefaultAsync(g => g.MaKH == maKH);

            if (gioHang == null)
            {
                gioHang = new GioHang { MaKH = maKH };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            // Add or update item
            var existingItem = gioHang.CT_GioHangs.FirstOrDefault(ct => ct.MaBienThe == maBienThe);
            if (existingItem != null)
            {
                existingItem.SoLuong += soLuong;
                existingItem.UocTinhThanhTien = existingItem.SoLuong * existingItem.GiaBan;
            }
            else
            {
                var newItem = new CT_GioHang
                {
                    MaGioHang = gioHang.MaGioHang,
                    MaBienThe = maBienThe,
                    SoLuong = soLuong,
                    GiaBan = bienThe.GiaBan,
                    UocTinhThanhTien = soLuong * bienThe.GiaBan
                };
                _context.CT_GioHangs.Add(newItem);
            }

            await _context.SaveChangesAsync();

            var totalItems = await _context.CT_GioHangs
                .Where(ct => ct.MaGioHang == gioHang.MaGioHang)
                .SumAsync(ct => ct.SoLuong);

            return Json(new { success = true, cartCount = totalItems, message = "Thêm vào giỏ thành công!" });
        }

        // ===== TRANG LIỆT KÊ GIỎ HÀNG =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem giỏ hàng.";
                return RedirectToAction("DangNhap", "Account");
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.CT_GioHangs)
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(b => b.MonAn)
                            .ThenInclude(m => m.GianHang)
                .FirstOrDefaultAsync(g => g.MaKH == maKH);

            return View(gioHang);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int maCTGioHang, int change)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return Json(new { success = false });

            var ctGioHang = await _context.CT_GioHangs
                .Include(c => c.GioHang)
                .FirstOrDefaultAsync(c => c.MaCTGioHang == maCTGioHang && c.GioHang!.MaKH == maKH);

            if (ctGioHang == null) return Json(new { success = false });

            ctGioHang.SoLuong += change;
            
            bool removed = false;
            decimal newPrice = 0;
            int newQuantity = 0;

            if (ctGioHang.SoLuong <= 0)
            {
                _context.CT_GioHangs.Remove(ctGioHang);
                removed = true;
            }
            else
            {
                ctGioHang.UocTinhThanhTien = ctGioHang.SoLuong * ctGioHang.GiaBan;
                newPrice = ctGioHang.UocTinhThanhTien;
                newQuantity = ctGioHang.SoLuong;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true,
                removed = removed,
                newPrice = newPrice,
                newQuantity = newQuantity
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int maCTGioHang)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return Json(new { success = false });

            var ctGioHang = await _context.CT_GioHangs
                .Include(c => c.GioHang)
                .FirstOrDefaultAsync(c => c.MaCTGioHang == maCTGioHang && c.GioHang!.MaKH == maKH);

            if (ctGioHang != null)
            {
                _context.CT_GioHangs.Remove(ctGioHang);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        // ===== XÁC NHẬN THANH TOÁN (CHECKOUT) =====
        [HttpGet]
        public async Task<IActionResult> Checkout(string selectedItems)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return RedirectToAction("DangNhap", "Account");

            if (string.IsNullOrEmpty(selectedItems))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một món để thanh toán!";
                return RedirectToAction("Index");
            }

            var selectedIds = selectedItems.Split(',').Select(int.Parse).ToList();

            var gioHang = await _context.GioHangs
                .Include(g => g.CT_GioHangs.Where(ct => selectedIds.Contains(ct.MaCTGioHang)))
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(b => b.MonAn)
                            .ThenInclude(m => m.GianHang)
                .FirstOrDefaultAsync(g => g.MaKH == maKH);

            if (gioHang == null || !gioHang.CT_GioHangs.Any())
            {
                TempData["Error"] = "Không tìm thấy các món đã chọn!";
                return RedirectToAction("Index");
            }

            ViewBag.SelectedItemsIds = selectedItems;
            var khachHang = await _context.KhachHangs.FindAsync(maKH);
            ViewBag.KhachHang = khachHang;

            // Tính toán phí giao hàng: 15.000đ mỗi quán
            var groupedItems = gioHang.CT_GioHangs.GroupBy(ct => ct.BienTheMonAn?.MonAn?.MaGianHang).ToList();
            ViewBag.TotalShippingFee = groupedItems.Count * 15000;

            // Lấy danh sách khuyến mãi còn hiệu lực
            var now = DateTime.Now;
            var khuyenMais = await _context.ChuongTrinhKhuyenMais
                .Where(k => k.NgayBatDau <= now && k.NgayKetThuc >= now)
                .ToListAsync();
            ViewBag.KhuyenMais = khuyenMais;

            return View(gioHang);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(string phuongThucThanhToan, string diaChiGiaoHang, int? maKM, string selectedItems)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return Json(new { success = false, message = "Lỗi đăng nhập." });

            if (string.IsNullOrEmpty(selectedItems))
                return Json(new { success = false, message = "Không có sản phẩm nào được chọn." });

            var selectedIds = selectedItems.Split(',').Select(int.Parse).ToList();

            var gioHang = await _context.GioHangs
                .Include(g => g.CT_GioHangs.Where(ct => selectedIds.Contains(ct.MaCTGioHang)))
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(b => b.MonAn)
                .FirstOrDefaultAsync(g => g.MaKH == maKH);

            if (gioHang == null || !gioHang.CT_GioHangs.Any())
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

            var itemsByStore = gioHang.CT_GioHangs
                .GroupBy(ct => ct.BienTheMonAn?.MonAn?.MaGianHang)
                .ToList();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var totalCartAmount = gioHang.CT_GioHangs.Sum(c => c.UocTinhThanhTien);
                decimal totalDiscount = 0;
                if (maKM.HasValue && maKM.Value > 0)
                {
                    var km = await _context.ChuongTrinhKhuyenMais.FindAsync(maKM.Value);
                    if (km != null)
                    {
                        totalDiscount = ((decimal)totalCartAmount * (decimal)km.PhanTramGiam) / 100;
                    }
                }

                var orderIds = new List<int>();
                int processedCount = 0;
                decimal accumulatedDiscount = 0;

                foreach (var storeGroup in itemsByStore)
                {
                    processedCount++;
                    var maGianHang = storeGroup.Key;
                    if (maGianHang == null) continue;

                    var storeItems = storeGroup.ToList();
                    var TongTienMon = storeItems.Sum(c => c.UocTinhThanhTien);
                    decimal GiamGia = 0;
                    decimal phiGiaoHangMotQuan = 15000; // Phí giao hàng cố định cho mỗi quán

                    // Phân bổ giảm giá theo tỷ lệ giá trị đơn hàng
                    if (totalDiscount > 0)
                    {
                        if (processedCount == itemsByStore.Count)
                        {
                            // Đơn hàng cuối cùng nhận phần còn lại để tránh sai số làm tròn
                            GiamGia = totalDiscount - accumulatedDiscount;
                        }
                        else
                        {
                            GiamGia = Math.Round((TongTienMon / totalCartAmount) * totalDiscount, 0);
                            accumulatedDiscount += GiamGia;
                        }
                    }

                    var thanhTienCuoi = TongTienMon - GiamGia + phiGiaoHangMotQuan;

                    // 1. Tạo đơn hàng
                    var donHang = new DonHang
                    {
                        MaKH = maKH,
                        MaGianHang = maGianHang.Value,
                        MaKMai = (GiamGia > 0) ? maKM : null,
                        TongTienMon = TongTienMon,
                        GiamGia = GiamGia,
                        PhiShip = phiGiaoHangMotQuan,
                        ThanhTienKhachTra = thanhTienCuoi,
                        TrangThaiDonHang = "Chờ xác nhận",
                        NgayTaoDon = DateTime.Now,
                        PhuongThucThanhToan = phuongThucThanhToan
                    };
                    
                    _context.DonHangs.Add(donHang);
                    await _context.SaveChangesAsync();
                    orderIds.Add(donHang.MaDonHang);

                    // 2. Tạo chi tiết đơn hàng
                    var ctDonHangs = storeItems.Select(ct => new ChiTietDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaBienThe = ct.MaBienThe,
                        SoLuongMua = ct.SoLuong,
                        GiaBan = ct.GiaBan
                    }).ToList();

                    _context.ChiTietDonHangs.AddRange(ctDonHangs);
                }

                // 3. Xoá Giỏ Hàng
                _context.CT_GioHangs.RemoveRange(gioHang.CT_GioHangs);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, message = $"Đặt hàng thành công! Đã tạo {orderIds.Count} đơn hàng cho các quán." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}








