with open('Views/Home/MonAnDetails.cshtml', 'r', encoding='utf-8') as f:
    text = f.read()

# Replace the GetDanhGia callback to handle new fields
old_load = """            $.getJSON(`/Home/GetDanhGia?maMonAn=${maMonAn}&page=${page}&sort=${sort}&filterStar=${filter}`, function(data) {
                renderReviews(data.reviews);
                renderPagination(data.currentPage, data.totalPages);
                renderStarBars(data.starSummary, data.totalCount);
            });"""

new_load = """            $.getJSON(`/Home/GetDanhGia?maMonAn=${maMonAn}&page=${page}&sort=${sort}&filterStar=${filter}`, function(data) {
                renderReviews(data.reviews);
                renderPagination(data.currentPage, data.totalPages);
                renderStarBars(data.starSummary, data.totalUnfilteredCount || data.totalCount);
                
                // Update filtered count text
                if (data.totalCount > 0) {
                    $('#totalReviewCount').parent().html(`<span id="totalReviewCount">${data.totalCount}</span> ĐÁNH GIÁ`);
                } else {
                    $('#totalReviewCount').parent().html(`<span id="totalReviewCount">0</span> ĐÁNH GIÁ`);
                }
            }).fail(function() {
                $('#reviewList').html('<div class="text-center py-5 text-muted"><i class="bi bi-exclamation-triangle fs-2 d-block mb-2 text-danger"></i> Lỗi khi tải đánh giá</div>');
            });"""

text = text.replace(old_load, new_load)

with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as f:
    f.write(text)

print("Updated JS callback")
