import os
import re
import shutil
import uuid
import requests
import config

# === MANUAL CREDENTIALS ===


# === CONSTANTS ===
PROTOBUF_CONTENT_TYPE = 'application/x-protobuf'
JSON_CONTENT_TYPE = 'application/json'
login_url = "https://beta.heroesgame.com/api/login"
account_play_url = "https://zz0.heroesofhistorygame.com/core/api/account/play"
startup_api_url = "https://zz1.heroesofhistorygame.com/game/startup"
wakeup_api_url = "https://zz1.heroesofhistorygame.com/game/wakeup"

# === HEADERS ===
def default_headers():
    return {"Content-Type": JSON_CONTENT_TYPE}


def bin_data_headers(session_data):
    return {
        "X-AUTH-TOKEN": session_data["sessionId"],
        "X-Request-Id": str(uuid.uuid4()),
        "X-Platform": "browser",
        "X-ClientVersion": session_data["clientVersion"],
        "Accept-Encoding": "gzip",
        "Content-Type": PROTOBUF_CONTENT_TYPE,
        "Accept": PROTOBUF_CONTENT_TYPE,
    }


def json_data_headers(session_data):
    headers = bin_data_headers(session_data)
    headers["Accept"] = JSON_CONTENT_TYPE
    return headers


# === NETWORK FUNCTIONS ===
def login():
    session = requests.Session()

    payload = {
        "username": config.USERNAME,
        "password": config.PASSWORD,
        "useRememberMe": False
    }

    response = session.post(login_url, headers=default_headers(), json=payload)
    response.raise_for_status()
    login_data = response.json()

    redirect_res = session.get(login_data["redirectUrl"])
    redirect_res.raise_for_status()

    client_version_match = re.search(r'const\s+clientVersion\s*=\s*"([^"]+)"', redirect_res.text)
    if not client_version_match:
        raise Exception("Client version not found.")

    client_version = client_version_match.group(1)

    play_payload = {
        "createDeviceToken": False,
        "meta": {
            "clientVersion": client_version,
            "device": "browser",
            "deviceHardware": "browser",
            "deviceManufacturer": "none",
            "deviceName": "browser",
            "locale": "en_DK",
            "networkType": "wlan",
            "operatingSystemName": "browser",
            "operatingSystemVersion": "1",
            "userAgent": "hoh-helper-mobile"
        },
        "network": "BROWSER_SESSION",
        "token": "",
        "worldId": None
    }

    res = session.post(account_play_url, headers=default_headers(), json=play_payload)
    res.raise_for_status()

    session_data = res.json()
    session_data["clientVersion"] = client_version
    return session_data


def get_bin_data(url, session_data):
    res = requests.post(url, headers=bin_data_headers(session_data))
    res.raise_for_status()
    return res.content


def get_json_data(url, session_data):
    res = requests.post(url, headers=json_data_headers(session_data))
    res.raise_for_status()
    return res.text


def save_bin_data(data, filename):
    save_path = os.path.join(config.DOWNLOAD_DIR, f'{filename}.bin')
    with open(save_path, 'wb') as file:
        file.write(data)


def save_json_data(data, filename):
    save_path = os.path.join(config.DOWNLOAD_DIR, f'{filename}.json')
    with open(save_path, 'w', encoding='utf-8') as file:
        file.write(data)


def reset_directories():
    if os.path.exists(config.DOWNLOAD_DIR):
        shutil.rmtree(config.DOWNLOAD_DIR)
    os.mkdir(config.DOWNLOAD_DIR)


# === MAIN FUNCTION ===
def main():
    reset_directories()
    print("Logging in...")
    session_data = login()
    print("Session data received")

    print("Fetching startup data...")
    data = get_bin_data(startup_api_url, session_data)
    save_bin_data(data, 'startup')
    data = get_json_data(startup_api_url, session_data)
    save_json_data(data, 'startup')
    print("Startup data saved")

    print("Fetching wakeup data...")
    data = get_bin_data(wakeup_api_url, session_data)
    save_bin_data(data, 'wakeup')
    data = get_json_data(wakeup_api_url, session_data)
    save_json_data(data, 'wakeup')
    print("Wakeup data saved")


if __name__ == "__main__":
    main()
