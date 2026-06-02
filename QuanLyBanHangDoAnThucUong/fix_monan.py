import os
import re

with open('Views/Home/MonAnDetails.cshtml', 'r', encoding='utf-8') as f:
    html = f.read()

# 1. Update rating display (soSao > 0)
old_rating_block = '''                        <div class="d-flex align-items-center mb-4 text-warning" style="font-size: 0.9rem;">
                            @if (soLuot > 0)
                            {
                                @for (int i = 1; i <= 5; i++)
                                {
                                    if (i <= Math.Round((decimal)soSao)) { <i class="bi bi-star-fill"></i> }
                                    else { <i class="bi bi-star"></i> }
                                }
                                <span class="text-dark fw-bold ms-2" style="font-size: 1.1rem;">@soSao.ToString("0.#")</span>
                            }
                            else
                            {
                                <span class="text-muted fst-italic">Chưa có đánh giá</span>
                            }
                            <span class="badge bg-secondary ms-auto">@Model.TheLoaiMonAn?.TenLoai</span>
                        </div>'''
new_rating_block = '''                        <div class="d-flex align-items-center mb-4 text-warning" style="font-size: 0.9rem;">
                            @if (soLuot > 0)
                            {
                                <span class="text-dark fw-bold me-2" style="font-size: 1.1rem;">@soSao.ToString("0.#")</span>
                                @for (int i = 1; i <= 5; i++)
                                {
                                    if (i <= Math.Round((decimal)soSao)) { <i class="bi bi-star-fill"></i> }
                                    else { <i class="bi bi-star"></i> }
                                }
                                <span class="text-muted ms-2">| @soLuot đánh giá</span>
                            }
                            else
                            {
                                <span class="text-muted fst-italic">Chưa có đánh giá</span>
                            }
                            <span class="badge bg-secondary ms-auto">@Model.TheLoaiMonAn?.TenLoai</span>
                        </div>'''
html = html.replace(old_rating_block, new_rating_block)

# 2. Extract Store Info Card
store_info_pattern = r'<!-- Store Info Card -->(.*?)<!-- ===== REVIEWS ===== -->'
store_info_match = re.search(store_info_pattern, html, flags=re.DOTALL)
store_info_content = store_info_match.group(1).strip()
html = html.replace(store_info_match.group(0), '<!-- ===== REVIEWS ===== -->')

# Remove LuotXem from Store Info
luotxem_pattern = r'<div class="text-center">\s*<div class="fw-bold" style="font-size: 0.9rem;">@\(Model\.GianHang\?\.LuotXem \?\? 0\)</div>\s*<div class="text-muted" style="font-size: 0.75rem;">Lượt xem</div>\s*</div>'
store_info_content = re.sub(luotxem_pattern, '', store_info_content)

# 3. Extract Reviews block
reviews_pattern = r'<!-- ===== REVIEWS ===== -->(.*?)</div>\s*</div>\s*@section Scripts'
reviews_match = re.search(reviews_pattern, html, flags=re.DOTALL)
reviews_content = reviews_match.group(1).strip()
html = html.replace(reviews_match.group(0), '</div>\n\n@section Scripts')

# Fix Filters position in Reviews block
filter_pattern = r'<!-- Sắp xếp & Lọc nằm ở đây góc phải nếu trên màn hình lớn, hoặc ở dưới nếu nhỏ -->\s*<div class="position-absolute top-0 end-0 me-3 mt-2 d-none d-lg-flex flex-column gap-2 align-items-end">(.*?)</div>\s*</div>\s*<div class="col-md-8 ps-md-5 mt-4 mt-md-0" id="starBars">'
filter_match = re.search(filter_pattern, reviews_content, flags=re.DOTALL)

if filter_match:
    filter_html = '<!-- Sắp xếp & Lọc nằm ở đây góc phải nếu trên màn hình lớn, hoặc ở dưới nếu nhỏ -->\n                    <div class="position-absolute top-0 end-0 me-3 mt-4 d-none d-lg-flex gap-2 justify-content-end" style="width: auto;">' + filter_match.group(1) + '</div>'
    
    # Remove from original place
    reviews_content = reviews_content.replace(filter_match.group(0), '</div>\n                <div class="col-md-8 ps-md-5 mt-4 mt-md-0" id="starBars">')
    
    # Add to the row container
    reviews_content = reviews_content.replace('<div class="row align-items-center mb-5 bg-white p-4 rounded-4 shadow-sm mx-0">', '<div class="row align-items-center mb-5 bg-white p-4 rounded-4 shadow-sm mx-0 position-relative">\n                    ' + filter_html)


# 4. Extract Similar Foods
similar_foods_pattern = r'<!-- 2\. Similar Foods -->\s*<div class="col-md-4 mb-4">(.*?)</div>\s*</div>\s*@section Scripts'
similar_foods_match = re.search(similar_foods_pattern, html, flags=re.DOTALL)
similar_foods_content = similar_foods_match.group(1).strip()
html = html.replace(similar_foods_match.group(0), '</div>\n\n@section Scripts')

# 5. Extract Tabs (Thành phần & Mô tả) and replace with blocks
old_tabs_block = '''                        <hr class="text-muted mt-auto" />
                        
                        <div class="mt-2">
                            <ul class="nav nav-pills gap-2 mb-3" role="tablist">
                                <li class="nav-item" role="presentation">
                                    <button class="nav-link active fw-bold px-4 rounded-pill" id="thanhphan-tab" data-bs-toggle="pill" data-bs-target="#thanhphan" type="button" role="tab">Thành phần</button>
                                </li>
                                <li class="nav-item" role="presentation">
                                    <button class="nav-link fw-bold px-4 rounded-pill text-dark bg-light" id="mota-tab" data-bs-toggle="pill" data-bs-target="#mota" type="button" role="tab" onclick="$(this).removeClass('text-dark bg-light').addClass('active').parent().siblings().find('button').removeClass('active').addClass('text-dark bg-light');">Mô tả</button>
                                </li>
                            </ul>
                            <div class="tab-content" style="word-wrap: break-word; overflow-wrap: anywhere;">
                                <div class="tab-pane fade show active text-muted small p-2" id="thanhphan" role="tabpanel">
                                    @(string.IsNullOrEmpty(Model.ThanhPhan) ? "Chưa có thông tin thành phần chi tiết." : Model.ThanhPhan)
                                </div>
                                <div class="tab-pane fade text-muted small p-2" id="mota" role="tabpanel">
                                    Món ăn được chuẩn bị với những nguyên liệu tươi ngon nhất, mang đến hương vị đậm đà và trải nghiệm tuyệt vời cho thực khách.
                                </div>
                            </div>
                        </div>'''
html = html.replace(old_tabs_block, '')

# Now the `col-md-8` ends. We insert the new blocks at the end of col-md-8.
left_col_end_pattern = r'                    </div>\s*</div>\s*</div>\s*</div>'
left_col_end_match = re.search(left_col_end_pattern, html)

replacement = '''                    </div>
                </div>
            </div>

            <!-- Thành phần & Mô tả -->
            <div class="card border border-light shadow-sm rounded-0 p-4 mb-4 bg-white">
                <h6 class="fw-bold text-dark mb-2">THÀNH PHẦN:</h6>
                <p class="text-muted small mb-4">@(string.IsNullOrEmpty(Model.ThanhPhan) ? "Chưa có thông tin thành phần chi tiết." : Model.ThanhPhan)</p>
                
                <h6 class="fw-bold text-dark mb-2">MÔ TẢ:</h6>
                <p class="text-muted small mb-0">Món ăn được chuẩn bị với những nguyên liệu tươi ngon nhất, mang đến hương vị đậm đà và trải nghiệm tuyệt vời cho thực khách.</p>
            </div>

            <!-- Store Info Card -->
            <div class="card border border-light shadow-sm mb-4 rounded-0 p-2">
                ''' + store_info_content + '''
            </div>

            <!-- ===== REVIEWS ===== -->
            <div class="card border-0 shadow-sm rounded-4 p-4 mt-4" id="reviewSection" style="background-color: #fafbfe;">
                ''' + reviews_content + '''
            </div>
        </div>

        <!-- Right Sidebar: Similar Foods -->
        <div class="col-md-4 mb-4">
            ''' + similar_foods_content + '''
        </div>
    </div>
'''

html = html.replace(left_col_end_match.group(0), replacement)

with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as f:
    f.write(html)
