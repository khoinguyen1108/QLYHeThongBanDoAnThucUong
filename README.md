TÀI LIỆU HƯỚNG DẪN CHI TIẾT CẤU TRÚC VÀ LUỒNG HOẠT ĐỘNG CỦA DỰ ÁN
Tài liệu này được biên soạn nhằm giải thích cặn kẽ cách hệ thống được xây dựng, từ nền tảng kỹ thuật đến các công thức toán học và thuật toán. Ngay cả người chưa có nhiều kinh nghiệm lập trình cũng có thể nắm bắt trọn vẹn luồng hoạt động của toàn bộ hệ thống.

1. KIẾN TRÚC TỔNG THỂ CỦA DỰ ÁN (CÁCH HỆ THỐNG VẬN HÀNH CHUNG)
Hệ thống được xây dựng dựa trên mô hình MVC (Model - View - Controller). Đây là một quy tắc phân chia công việc trong lập trình giúp hệ thống chạy ổn định và dễ quản lý:

Model (Dữ liệu): Là nơi chứa toàn bộ thông tin được lưu trữ trong Cơ sở dữ liệu (ví dụ: Danh sách món ăn, Thông tin người dùng, Lịch sử hóa đơn).
View (Giao diện): Là phần hiển thị trực quan mà người dùng nhìn thấy (các nút bấm, hình ảnh, văn bản). Phần này dùng HTML, CSS và gói thư viện Bootstrap 5 để sắp xếp bố cục đẹp mắt.
Controller (Bộ điều khiển): Đóng vai trò là "Bộ não trung gian". Khi một người nhấn nút "Mua hàng" trên màn hình (View), tín hiệu sẽ được truyền về Controller. Controller sẽ lấy thông tin món ăn từ Model, thực hiện các phép tính toán (như tính tổng tiền), sau đó ra lệnh cho View hiển thị kết quả cuối cùng lên màn hình.
Bên cạnh đó, dự án ứng dụng công nghệ AJAX (Asynchronous JavaScript and XML) cho các thao tác trên giao diện. AJAX cho phép hệ thống "giao tiếp ngầm" với máy chủ dữ liệu mà không cần tải lại (refresh) toàn bộ trang web. Nhờ vậy, trải nghiệm khi tìm kiếm, thêm vào giỏ hàng hay cập nhật trạng thái diễn ra tức thời và mượt mà giống như các ứng dụng trên điện thoại di động.

2. CHI TIẾT LUỒNG HOẠT ĐỘNG Ở GIAO DIỆN KHÁCH HÀNG
Giao diện khách hàng là nơi xử lý các thuật toán phức tạp nhất nhằm tối ưu trải nghiệm mua sắm.

A. Luồng Tìm kiếm và Bộ lọc
Cách thức hoạt động: Khi gõ một từ khóa vào ô tìm kiếm, công nghệ AJAX sẽ bắt từng ký tự được gõ và gửi thẳng về máy chủ. Máy chủ tiến hành so khớp từ khóa này với 3 vùng dữ liệu: Tên gian hàng, Tên món ăn và Thành phần món ăn.
Kết quả: Dữ liệu khớp sẽ được trả về và ngay lập tức hiển thị dưới dạng danh sách xổ xuống mà không cần chờ chuyển trang.
Bộ lọc đa luồng: Sau khi tìm kiếm, hệ thống cho phép chọn tiếp các điều kiện thu hẹp như Thành phố, Phường/Xã. Kết quả cuối cùng sẽ được sắp xếp bằng lệnh truy vấn trực tiếp vào cơ sở dữ liệu để tìm ra các món có Lượt bán cao nhất, hoặc Giá thấp nhất.
B. Thuật toán Gợi ý Món ăn tương tự (Recommendation Scoring Algorithm)
Khi đang xem một món ăn, hệ thống sẽ tính toán để đưa ra các món có liên quan nhất. Quá trình này áp dụng thuật toán chấm điểm (Scoring Algorithm) với công thức toán học cụ thể.

Bước 1: Lọc rào cản phân loại Hệ thống tự động phát hiện từ khóa "chay" trong tên hoặc thành phần. Nếu món đang xem là món chay, rào cản sẽ chặn mọi món mặn và ngược lại.

Bước 2: Phân tách từ khóa Trích xuất tên món ăn thành một tập hợp các từ khóa độc lập. Gọi tập hợp này là $K$. Hệ thống sẽ loại bỏ các từ nối vô nghĩa (như "của", "với", "và").

Bước 3: Công thức tính điểm ($S$) cho từng món ăn đề xuất Với mỗi món ăn đang được xem xét để đề xuất, hệ thống sẽ tính tổng điểm $S$ dựa trên công thức sau:

$$S = Match(C_1, C_2) + \sum_{k \in K} KeywordScore(k)$$

Trong đó:

$C_1$ là Thể loại của món đang đề xuất, $C_2$ là Thể loại của món đang xem.
Hàm $Match(C_1, C_2) = 10$ (nếu cùng thể loại), ngược lại bằng $0$.
Hàm $KeywordScore(k) = 5$ (nếu tên món đề xuất chứa từ khóa $k$ thuộc tập hợp $K$), ngược lại bằng $0$.
Kết quả: Hệ thống tính điểm $S$ cho toàn bộ cơ sở dữ liệu. Món nào có điểm $S$ cao nhất sẽ được đưa lên đầu danh sách gợi ý. Nếu điểm bằng nhau, món nào có số lượt bán lớn hơn sẽ xếp trên.

C. Luồng Giỏ hàng và Công thức Thanh toán
Giỏ hàng được thiết kế theo luật: Mỗi lần thanh toán chỉ áp dụng cho món ăn thuộc về một gian hàng duy nhất. Điều này giúp luồng đơn hàng không bị tách mảnh, đối tác dễ dàng chuẩn bị món.

Công thức toán học tính tiền thanh toán: Gọi $P_i$ là giá bán của món hàng thứ $i$. Gọi $Q_i$ là số lượng của món hàng thứ $i$. Gọi $D$ là số tiền được giảm giá thông qua Khuyến mãi. Gọi $F$ là phí vận chuyển.

Tính Tổng tiền hàng (Subtotal): $$Subtotal = \sum_{i=1}^{n} (P_i \times Q_i)$$ (Tổng tiền hàng bằng tổng cộng của giá bán nhân với số lượng của từng món).

Tính Số tiền giảm giá ($D$): Khi áp dụng mã khuyến mãi, hệ thống kiểm tra ràng buộc (còn hạn, tổng tiền hàng có đạt mức tối thiểu chưa). Nếu mã giảm giá quy định giảm theo phần trăm ($x%$): $$D = Subtotal \times \left( \frac{x}{100} \right)$$ (Hệ thống luôn có bước giới hạn số tiền $D$ tối đa theo quy định của mã).

Tính Tổng thanh toán cuối cùng (Total): $$Total = Subtotal - D + F$$

Hóa đơn sẽ chốt mức giá Total này và lưu vào Cơ sở dữ liệu cùng với Phương thức thanh toán được chọn (Thanh toán khi nhận hàng, thẻ tín dụng, v.v.).

CÁCH XÂY DỰNG LUỒNG THANH TOÁN MOMO TRONG DỰ ÁN
1. Cơ chế hoạt động và Thuật toán hiển thị
Khi khách hàng tiến hành thanh toán giỏ hàng, giao diện lựa chọn hình thức thanh toán được vẽ ra với hai tùy chọn: "Thanh toán tiền mặt khi nhận hàng (COD)" và "Thanh toán qua Ví MoMo".

Khi khách hàng bấm nút "Xác Nhận Đặt Hàng", thuật toán JavaScript ở phía giao diện sẽ rẽ nhánh (if/else) dựa theo phương thức thanh toán đã chọn:

Nếu là "Tiền mặt": Lập tức chuyển dữ liệu lên Server để tạo đơn hàng.
Nếu là "Ví điện tử": Chặn tiến trình tạo đơn hàng và kích hoạt Luồng giả lập thanh toán (MoMo Mockup Flow).
2. Luồng giả lập thanh toán (MoMo Mockup Flow)
Luồng này được lập trình trực tiếp trên file Checkout.cshtml bằng cách sử dụng thư viện SweetAlert2 kết hợp JavaScript:

Bước 1: Trích xuất và truyền dữ liệu hóa đơn Hệ thống JavaScript sẽ đọc giá trị finalTotal (Tổng tiền cuối cùng sau khi đã cộng phí vận chuyển và trừ khuyến mãi).

Bước 2: Hiển thị giao diện quét mã Mã lệnh gọi thư viện SweetAlert2 mở một hộp thoại (Modal) được tùy chỉnh lại giao diện bằng HTML. Hộp thoại này chứa logo của MoMo, thông báo số tiền cần thanh toán chính xác bằng với finalTotal, và một ô nhập liệu yêu cầu người dùng điền "Số tiền đã chuyển khoản".

Bước 3: Thuật toán Xác thực số tiền (Validation) Đây là công đoạn mô phỏng lại quá trình đối soát dữ liệu (Reconciliation) của một cổng thanh toán thực tế. Khi người dùng bấm nút "Đã chuyển khoản", mã lệnh sẽ chạy một hàm preConfirm chặn lại để kiểm tra:

Đọc số tiền người dùng vừa điền vào ô xác nhận.
Kiểm tra rỗng: Nếu chưa nhập, hệ thống chặn lại và báo lỗi "Vui lòng nhập số tiền đã chuyển khoản".
Phép toán đối soát: Khởi tạo biến $M_{input}$ là số tiền nhập vào và $Total$ là số tiền phải thanh toán.
Nếu $M_{input} < Total$, hệ thống đưa ra cảnh báo "Số tiền chưa chuyển đủ và cần chuyển lại." và từ chối xác nhận.
Nếu $M_{input} \ge Total$, hệ thống đánh giá giao dịch hợp lệ.
Bước 4: Hoàn tất thanh toán và Tạo đơn hàng Sau khi xác thực thành công (giả lập việc MoMo trả về trạng thái báo giao dịch thành công), giao diện hiển thị thông báo "Chuyển khoản thành công!". Sau độ trễ 1.5 giây, mã lệnh mới tiếp tục gọi hàm AJAX (truyền mã phương thức là "Ví điện tử") đẩy toàn bộ dữ liệu địa chỉ, mã khuyến mãi về Server (/Cart/ProcessCheckout) để lưu vào Cơ sở dữ liệu và chuyển trạng thái đơn hàng thành Chờ xác nhận.

Mục đích của phương pháp này
Do hệ thống được phát triển trong khuôn khổ đồ án môn học, việc dùng tài khoản doanh nghiệp thực tế để lấy chuỗi bảo mật (Secret Key) và sinh mã hóa thuật toán HMAC_SHA256 gửi lên Server của MoMo là không khả thi. Do đó, việc xây dựng luồng giả lập bằng JavaScript/SweetAlert2 nhằm đạt được 2 mục tiêu:

Tái hiện lại trọn vẹn trải nghiệm người dùng (UX) khi chọn thanh toán bằng ví điện tử.
Đảm bảo luồng kiểm tra logic số dư hóa đơn vẫn hoạt động độc lập mà không bị gián đoạn tiến trình mua hàng
1. GIAO DIỆN VÀ TÍNH NĂNG ĐỐI TÁC (PARTNER)
Khu vực này được xây dựng độc lập, cô lập dữ liệu hoàn toàn so với khách hàng và các đối tác khác.

A. Quản lý Tài khoản và Điều lệ (Business Rules)
Cơ chế xác thực (Authentication): Khi đăng nhập, hệ thống sử dụng công nghệ cấp thẻ (Session/Cookie) mang nhãn Role = "DoiTac". Bất kỳ yêu cầu truy cập nào vào khu vực này đều phải đi qua "cửa kiểm duyệt" (Attribute [Authorize]). Nếu mã định danh không hợp lệ, hệ thống tự động đẩy người dùng ra ngoài.
Áp dụng Điều lệ hệ thống: Điều lệ kinh doanh không phải là một văn bản đơn thuần mà được chuyển hóa thành các ràng buộc mã lệnh (Hard-coded Logic).
Ví dụ: Đối tác đăng ký mới mặc định bị khóa (Trạng thái = Chờ duyệt). Mã lệnh C# sẽ chặn mọi hành vi thêm món ăn hay mở cửa hàng cho đến khi cột TrangThai trong cơ sở dữ liệu được Admin đổi thành Đã duyệt.
Đối tác có công tắc "Mở cửa/Đóng cửa". Mã lệnh sẽ kiểm tra trạng thái này; nếu là "Đóng cửa", mọi nút "Thêm vào giỏ hàng" đối với các món ăn của gian hàng đó trên giao diện khách sẽ tự động vô hiệu hóa.
B. Quản lý Khuyến mãi (Promotions)
Cách xây dựng: Cung cấp biểu mẫu (Form) để đối tác tạo mã giảm giá. Dữ liệu lưu vào bảng KhuyenMai, bao gồm các thuộc tính bắt buộc: Mã code, Số tiền/Phần trăm giảm, Điều kiện đơn tối thiểu, Ngày bắt đầu và Ngày kết thúc.
Thuật toán áp dụng: Khi khách hàng nhập mã, hệ thống C# thực hiện kiểm tra 3 rào cản logic:
Thời gian: $T_{Start} \le T_{Current} \le T_{End}$.
Ràng buộc hóa đơn: $Total_{Cart} \ge MinOrderCondition$.
Tính toán trừ tiền:
Dạng phần trăm ($x%$): $Discount = Total_{Cart} \times \left( \frac{x}{100} \right)$
Dạng tiền mặt: $Discount = x$
Kiểm soát giới hạn: Nếu $Discount > MaxDiscountLevel$ (Mức giảm tối đa cho phép), thì $Discount = MaxDiscountLevel$.
2. GIAO DIỆN VÀ TÍNH NĂNG QUẢN TRỊ VIÊN (ADMIN)
Đây là tầng quản lý cao nhất (Nhãn Role = "Admin"), cho phép bao quát toàn bộ tài nguyên trên máy chủ.

A. Sao kê tài khoản và Thống kê doanh thu (Account Statement & Ledger)
Cách hiển thị dữ liệu: Vì dữ liệu sao kê của toàn bộ gian hàng là rất lớn, mã lệnh áp dụng công nghệ jQuery DataTables kết hợp với AJAX. Thay vì tải một bảng danh sách khổng lồ gây treo máy, hệ thống chỉ yêu cầu máy chủ gửi về từng khối 10-20 dòng dữ liệu một lúc (Server-side processing).
Thuật toán gom nhóm (Grouping): Để ra được bảng sao kê doanh thu cho từng gian hàng, mã lệnh C# sử dụng vòng lặp (hoặc LINQ GroupBy).
Hệ thống gom tất cả Đơn hàng có chung Mã Gian Hàng và có trạng thái là Hoàn thành.
Cộng dồn cột Tổng Tiền của các đơn hàng đó để ra được Doanh thu tổng.
Theo dõi dòng tiền: Bảng sao kê cung cấp cho Admin góc nhìn đối soát: Gian hàng A đã thu được tổng cộng bao nhiêu tiền, từ đó phục vụ cho việc tính chiết khấu (nếu có) hoặc xuất báo cáo tài chính hàng tháng.
B. Quản lý Đối tác và Quản lý Đánh giá
Quản lý Đối tác: Giao diện hiển thị danh sách dạng lưới. Mã lệnh lấy thông tin từ bảng DoiTac kết hợp (Join) với bảng GianHang. Admin có nút "Khóa tài khoản" hoặc "Duyệt". Thao tác này kích hoạt câu lệnh UPDATE vào cơ sở dữ liệu, ngay lập tức tước quyền truy cập của đối tác vi phạm.
Quản lý Đánh giá toàn cục: Admin sở hữu một giao diện cho phép truy xuất mọi đánh giá trên hệ thống.
Mã lệnh C# sẽ nối 3 bảng dữ liệu: Khách Hàng (người đánh giá) - Đánh Giá (nội dung) - Gian Hàng (nơi nhận đánh giá).
Admin có công cụ bộ lọc (Filter) để lọc ra các đánh giá "1 sao" nhằm thanh tra chất lượng của đối tác, hoặc xóa các bình luận chứa từ ngữ không chuẩn mực mà không cần sự đồng ý của đối tác.
C. Bảng điều khiển (Dashboard)
Xây dựng bằng thư viện đồ họa Chart.js. Máy chủ C# đóng vai trò là "máy tính toán", tổng hợp số lượng Đơn hàng, số lượng Đối tác mới trong 30 ngày qua thành các dãy số (Mảng - Array). Các dãy số này được nạp vào cấu trúc JavaScript của Chart.js để vẽ thành biểu đồ cột và biểu đồ đường, giúp Admin nhìn thấy đỉnh và đáy của lưu lượng giao dịch.
3. CÔNG THỨC TOÁN HỌC TRỌNG TÂM
Thuật toán tính Số sao trung bình (Star Rating Formula)
Số sao trung bình hiển thị trên từng món ăn không phải do con người tự nhập mà được tính toán tự động và nghiêm ngặt.

Công thức toán học: Gọi $N$ là tổng số lượt đánh giá của một món ăn. Gọi $S_i$ là số điểm sao của lượt đánh giá thứ $i$ ($S_i \in {1, 2, 3, 4, 5}$). Điểm sao trung bình ($\bar{S}$) được tính bằng công thức trung bình cộng:

$$ \bar{S} = \frac{1}{N} \sum_{i=1}^{N} S_i $$

Cách lập trình và tối ưu hóa: Thay vì để hệ thống web (C#) phải chạy vòng lặp tính toán lại công thức trên mỗi khi có người mở xem trang web (gây chậm máy chủ), hệ thống áp dụng kỹ thuật Database Trigger (Khởi chạy tự động cấp độ Cơ sở dữ liệu).

Trigger trg_UpdateDanhGiaMonAn: Một đoạn mã lệnh được "cấy" thẳng vào bảng DanhGiaMonAn trong SQL Server.
Ngay khi một khách hàng ấn nút gửi đánh giá mới, hệ thống cơ sở dữ liệu sẽ tự động đánh thức đoạn mã này, tính toán lại công thức $\bar{S}$ và lập tức ghi đè kết quả mới nhất vào cột SoSaoTrungBinh của món ăn tương ứng.
Nhờ vậy, khi hiển thị ra giao diện, mã nguồn chỉ việc đọc thẳng con số cuối cùng (ví dụ: $4.5$ sao) mà không tốn bất kỳ tài nguyên tính toán dư thừa nào. Công thức này đảm bảo độ phản hồi tức thời và tính nhất quán dữ liệu tuyệt đối.
