import os
import re

with open('resources/SQLQuery.sql', 'r', encoding='utf-8') as f:
    sql = f.read()

sql = sql.replace('DanhGiaTB = 0', "DanhGiaGianHang = '0'")
sql = sql.replace('SET DanhGiaTB = COALESCE', 'SET DanhGiaGianHang = COALESCE')

with open('resources/SQLQuery.sql', 'w', encoding='utf-8') as f:
    f.write(sql)
