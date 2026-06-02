import json

log_path = r'C:\Users\Admin\.gemini\antigravity\brain\8687b864-5d51-46e6-9a5d-ad6866b6671f\.system_generated\logs\transcript.jsonl'

with open(log_path, 'r', encoding='utf-8') as f:
    for line in f:
        try:
            data = json.loads(line)
            if data.get('type') == 'RUN_COMMAND':
                content = data.get('content', '')
                if 'SQLQuery.sql' in content:
                    print(f"Step {data.get('step_index')} command: {content}")
        except:
            pass
