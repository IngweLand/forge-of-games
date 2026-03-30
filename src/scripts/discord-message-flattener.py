import json
from pathlib import Path


def extract_message(msg):
    author = msg.get("author") or {}
    return {
        "content": msg.get("content"),
        "mentions": msg.get("mentions"),
        "timestamp": msg.get("timestamp"),
        "id": msg.get("id"),
        "channel_id": msg.get("channel_id"),
        "author_id": author.get("id"),
        "author_username": author.get("username"),
        "author_global_name": author.get("global_name"),
    }


def main(input_folder, output_file):
    input_path = Path(input_folder)

    seen_ids = set()
    result = []

    for file in input_path.glob("*.json"):
        with open(file, "r", encoding="utf-8") as f:
            data = json.load(f)

        for msg in data:
            msg_id = msg.get("id")
            if not msg_id or msg_id in seen_ids:
                continue

            seen_ids.add(msg_id)
            result.append(extract_message(msg))

    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(result, f, ensure_ascii=False, indent=2)


if __name__ == "__main__":
    main(r"D:/Temp/discord-messages", r"D:/Temp/discord-messages/result.json")
