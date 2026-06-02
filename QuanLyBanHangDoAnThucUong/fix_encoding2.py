import re

with open('Views/Home/MonAnDetails.cshtml', 'r', encoding='utf-8') as f:
    text = f.read()

# Common mangled words in this specific file
replacements = {
    'Trang chÃ¡Â»Â§': 'Trang chủ',
    'ThÃnh pháº§n': 'Thành phần',
    'MÃ´ táº£': 'Mô tả',
    'THÃ€NH PHáº¦N': 'THÀNH PHẦN',
    'MÃ” Táº¢': 'MÔ TẢ',
    'MÃ³n Äƒn Ä‘Æ°á»£c chuáº©n bá»‹ vá»›i nhá»¯ng nguyÃªn liá»‡u tÆ°Æ¡i ngon nháº¥t, mang Ä‘áº¿n hÆ°Æ¡ng vá»‹ Ä‘áºm Ä‘Ã vÃ tráº£i nghiá»‡m tuyá»‡t vá» i cho thá»±c khÃ¡ch.': 'Món ăn được chuẩn bị với những nguyên liệu tươi ngon nhất, mang đến hương vị đậm đà và trải nghiệm tuyệt vời cho thực khách.',
    'ChÆ°a cÃ³ thÃ´ng tin thÃnh pháº§n chi tiáº¿t.': 'Chưa có thông tin thành phần chi tiết.',
    'Ä Ã¡nh giÃ¡': 'Đánh giá',
    'Má»Ÿ cá»­a': 'Mở cửa',
    'Tráº¡ng thÃ¡i': 'Trạng thái',
    'Xem gian hÃ ng': 'Xem gian hàng',
    'sáº£n pháº©m': 'sản phẩm',
    'Sáº¯p xáº¿p': 'Sắp xếp',
    'Má»›i nháº¥t': 'Mới nhất',
    'CÅ© nháº¥t': 'Cũ nhất',
    'Sao cao nháº¥t': 'Sao cao nhất',
    'Sao tháº¥p nháº¥t': 'Sao thấp nhất',
    'Lá» c sao': 'Lọc sao',
    'Táº¥t cáº£ sao': 'Tất cả sao',
    'trÃªn 5': 'trên 5',
    'Ä Ã NH GIÃ ': 'ĐÁNH GIÁ',
    'ChÆ°a cÃ³ Ä‘Ã¡nh giÃ¡': 'Chưa có đánh giá',
    'Ä ang táº£i Ä‘Ã¡nh giÃ¡...': 'Đang tải đánh giá...',
    'Viáº¿t Ä‘Ã¡nh giÃ¡ cá»§a báº¡n': 'Viết đánh giá của bạn',
    'Tráº£i nghiá»‡m cá»§a báº¡n tháº¿ nÃo?': 'Trải nghiệm của bạn thế nào?',
    'HÃ£y chia sáº» cáº£m nháºn chi tiáº¿t cá»§a báº¡n vá»  mÃ³n Äƒn nÃy nhÃ©...': 'Hãy chia sẻ cảm nhận chi tiết của bạn về món ăn này nhé...',
    'Gá»i Ä Ã¡nh GiÃ¡': 'Gửi Đánh Giá',
    'Báº¡n chÆ°a thá»ƒ Ä‘Ã¡nh giÃ¡ mÃ³n Äƒn nÃ y': 'Bạn chưa thể đánh giá món ăn này',
    'Báº¡n chá»‰ cÃ³ quyá» n Ä‘Ã¡nh giÃ¡ sau khi Ä‘Ã£ mua vÃ hoÃn thÃnh Ä‘Æ¡n hÃng chá»©a mÃ³n Äƒn nÃy.': 'Bạn chỉ có quyền đánh giá sau khi đã mua và hoàn thành đơn hàng chứa món ăn này.',
    'MÃ³n tÆ°Æ¡ng tá»±': 'Món tương tự',
    'Háº¿t hÃng': 'Hết hàng',
    'Pháº£n há»“i tá»« quÃ¡n': 'Phản hồi từ quán',
    'XÃ³a Ä‘Ã¡nh giÃ¡': 'Xóa đánh giá',
    'XÃ³a': 'Xóa',
    'KhÃ´ng cÃ³ ná»™i dung Ä‘Ã¡nh giÃ¡': 'Không có nội dung đánh giá',
    'Sá»‘ lÆ°á»£ng': 'Số lượng',
    'ThÃªm vÃ o giá»  hÃ ng': 'Thêm vào giỏ hàng',
    'Ä‘Ã¡nh giÃ¡': 'đánh giá',
    'TÃ¹y chá» n': 'Tùy chọn',
    'KÃ­ch cá»¡': 'Kích cỡ',
    'gian hÃng': 'gian hàng',
    'Ä‘áº¿n': 'đến',
    'tuyá»‡t vá» i': 'tuyệt vời',
    'khÃ¡ch': 'khách',
    'chuáº©n bá»‹': 'chuẩn bị',
    'nguyÃªn liá»‡u': 'nguyên liệu',
    'tÆ°Æ¡i ngon': 'tươi ngon',
    'hÆ°Æ¡ng vá»‹': 'hương vị',
    'Ä‘áºm Ä‘Ã': 'đậm đà',
    'tráº£i nghiá»‡m': 'trải nghiệm',
}

for bad, good in replacements.items():
    text = text.replace(bad, good)

# Fix numbers like "Ä‘" -> "đ"
text = text.replace('Ä‘', 'đ')

# Write back
with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as f:
    f.write(text)

print("Replacement complete")
