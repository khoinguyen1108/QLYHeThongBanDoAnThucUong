import json
import re

log_path = r"C:\Users\Admin\.gemini\antigravity\brain\8687b864-5d51-46e6-9a5d-ad6866b6671f\.system_generated\logs\transcript.jsonl"
with open(log_path, 'r', encoding='utf-8') as f:
    for line in f:
        try:
            step = json.loads(line)
            if step.get('type') == 'TOOL_RESPONSE' or 'TOOL' in step.get('type', ''):
                content = step.get('content', '')
                if 'default_api:view_file' in str(step) and 'Views/Home/MonAnDetails.cshtml' in str(step):
                    # extract lines
                    lines = []
                    for outline in content.split('\n'):
                        match = re.match(r'^\d+:\s(.*)$', outline)
                        if match:
                            lines.append(match.group(1))
                        elif re.match(r'^\d+:$', outline):
                            lines.append('')
                    
                    if len(lines) > 500:
                        with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as outf:
                            outf.write('\n'.join(lines))
                        print(f"Successfully recovered {len(lines)} lines!")
                        exit(0)
        except Exception as e:
            pass

# Fallback: just search for the string "The following code has been modified to include a line number before every line" in any step
with open(log_path, 'r', encoding='utf-8') as f:
    for line in f:
        try:
            step = json.loads(line)
            content = step.get('content', '')
            if 'The following code has been modified to include a line number' in content and 'MonAnDetails.cshtml' in content:
                lines = []
                for outline in content.split('\n'):
                    match = re.match(r'^\d+:\s(.*)$', outline)
                    if match:
                        lines.append(match.group(1))
                    elif re.match(r'^\d+:$', outline):
                        lines.append('')
                
                if len(lines) > 500:
                    with open('Views/Home/MonAnDetails.cshtml', 'w', encoding='utf-8') as outf:
                        outf.write('\n'.join(lines))
                    print(f"Successfully recovered {len(lines)} lines from fallback!")
                    exit(0)
        except:
            pass

print("Could not find suitable view_file response.")
