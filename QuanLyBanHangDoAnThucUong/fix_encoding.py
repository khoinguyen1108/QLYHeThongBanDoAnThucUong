with open('Views/Home/MonAnDetails.cshtml', 'r', encoding='utf-8') as f:
    text = f.read()

try:
    # Reverse the double-encoding:
    # 1. PowerShell read UTF-8 bytes as CP1252 (or default ANSI, maybe CP1258).
    # 2. Then wrote them out as UTF-8.
    # So the current text is "MÃ³n", which is M, \xc3, \xb3, n.
    # If we encode as CP1252, we get the original bytes back.
    # Then decode as utf-8.
    original = text.encode('cp1252').decode('utf-8')
    with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as f:
        f.write(original)
    print("Success CP1252")
except Exception as e:
    print("Failed CP1252:", e)
    try:
        original = text.encode('cp1258').decode('utf-8')
        with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as f:
            f.write(original)
        print("Success CP1258")
    except Exception as e2:
        print("Failed CP1258:", e2)

