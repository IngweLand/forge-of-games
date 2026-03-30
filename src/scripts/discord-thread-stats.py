import json
import os
from collections import Counter

folder = r"D:/Temp/discord-messages"  # change this to your folder path

all_messages = {}  # keyed by message id to deduplicate

for filename in os.listdir(folder):
    if not filename.endswith(".json"):
        continue
    with open(os.path.join(folder, filename), "r", encoding="utf-8") as f:
        messages = json.load(f)
    for msg in messages:
        msg_id = msg["id"]
        if msg_id not in all_messages:
            all_messages[msg_id] = msg["author"]["global_name"] or msg["author"]["username"]

usernames = list(all_messages.values())
counts = Counter(usernames)

print(f"Total users: {len(counts)}")
print(f"Total messages: {len(all_messages)}")
print()

for user, count in counts.most_common():
    print(f"{user}: {count}")