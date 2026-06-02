with open('resources/SQLQuery.sql', 'r', encoding='utf-8') as f:
    text = f.read()

text = text.replace('EmailKH NVARCHAR(255) NOT NULL,', 'EmailKH NVARCHAR(255) NOT NULL,\n    Avatar NVARCHAR(255) NULL,')

with open('resources/SQLQuery.sql', 'w', encoding='utf-8') as f:
    f.write(text)

print("Added Avatar to KhachHang in SQLQuery.sql")
