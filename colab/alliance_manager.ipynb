{
  "nbformat": 4,
  "nbformat_minor": 0,
  "metadata": {
    "colab": {
      "provenance": []
    },
    "kernelspec": {
      "name": "python3",
      "display_name": "Python 3"
    },
    "language_info": {
      "name": "python"
    }
  },
  "cells": [
    {
      "cell_type": "markdown",
      "source": [
        "# **Alliance Manager** - Forge of Games\n",
        "\n",
        "**README**\n",
        "\n",
        "1.   [One time] Save this Colab notebook to your Google Drive (File > Save a copy in Drive). This will create a copy of this document and will open it in a new browser tab. Close this tab and proceed to your saved copy.\n",
        "2.   Create new Google Spreadsheet document\n",
        "3.   Update following properties in the `Configs` section below:\n",
        "      *   `SPREADSHEET_ID`, `ALLIANCE_NAME`, `CUTOFF_DATE`, `GAME_WORLD_ID` must be set just **once**\n",
        "      *   `ALLOW_ALLIANCE_ATH_LEADERBOARD_EXPORT` can be updated any time\n",
        "4.    Open *Secrets* by clicking on a *key* icon in the left menu. Add 2 secrets: `HOH_USERNAME` and `HOH_PASSWORD`. Enter your real game login credentials as their values. Toggle *Notebook access* for both of them.\n",
        "5.    **Important!** The script works by signing in to your game account and fetching required data. Then the data is sent to Forge of Games server for parsing. List of parsed player rankings is returned back to the script. Note, your game credentials are **never** sent to the Forge of Games. The server **does not** save or analyze data. However, you can allow sharing of treasure hunt alliance leadeboard by setting `ALLOW_ALLIANCE_ATH_LEADERBOARD_EXPORT` to `True` in the `Configs` section below. By doing so, you provide data for the *Treasure Hunt Ranking* section of the Alliance profile on Forge of Games. Even with this sharing enabled, the server **does not** analyze or share your internal player rankings.\n",
        "6.    Click *Run all* button above. Colab environment will request access to your Google Drive in a new windows. Allow everything.\n",
        "7.    While script is running, scroll to the bottom of the code. Notice the log messages popping up as the script is executed. Wait until you see *DONE*. Then, go to your spreadsheet document and check the results.\n",
        "8.    When you need to update the data, simply click on *Run all* again. If you closed this notebook document, open it from your Google Drive.\n",
        "9.    The tool is open-source. Contributions are welcome.\n",
        "10.    If you find this tool useful, consider supporting Forge of Games project to benefit the whole game community. Details how to donate can be found here https://forgeofgames.com/support-us\n"
      ],
      "metadata": {
        "id": "45MUIWDQrmIF"
      }
    },
    {
      "cell_type": "code",
      "metadata": {
        "id": "3be3c4d1"
      },
      "source": [
        "!pip install gspread oauth2client"
      ],
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Imports**"
      ],
      "metadata": {
        "id": "VKjmtB3sbP0k"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "from google.colab import auth\n",
        "from google.auth import default\n",
        "from google.colab import userdata\n",
        "from datetime import datetime\n",
        "from gspread.utils import a1_range_to_grid_range, a1_to_rowcol, rowcol_to_a1\n",
        "from dataclasses import dataclass\n",
        "from typing import List\n",
        "import string\n",
        "import gspread\n",
        "import requests\n",
        "import re\n",
        "import uuid\n",
        "import base64"
      ],
      "metadata": {
        "id": "RMIqRdKTrsrV"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Configs**"
      ],
      "metadata": {
        "id": "9X6u89gzbSZl"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "# You can find the Spreadsheet ID in the URL of your Google Sheet. It's the long string of characters between '/d/' and '/edit'.\n",
        "SPREADSHEET_ID = ''\n",
        "\n",
        "# Will become the name of the worksheet in your Spreadsheet document.\n",
        "ALLIANCE_NAME = 'My Alliance'\n",
        "\n",
        "# The earliest date from which events should be considered\n",
        "# Normally, you would set it to the start date of the currently running ATH campaign\n",
        "CUTOFF_DATE = datetime(2025, 10, 30)  # year, month, day\n",
        "\n",
        "# 'un1' for main server or 'zz1' for beta server\n",
        "GAME_WORLD_ID = 'un1'\n",
        "\n",
        "# Change to True if you want to share treasure hunt alliance leaderboard with Forge of Games.\n",
        "# Note, your internal players' treasure hunt points will NOT be shared.\n",
        "ALLOW_ALLIANCE_ATH_LEADERBOARD_EXPORT = False"
      ],
      "metadata": {
        "id": "podlWA1jqeBh"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "FOG_API_URL_BASE = 'https://forgeofgames.com/api/hoh'\n",
        "FOG_API_EVENTS = FOG_API_URL_BASE + '/inGameEvents/{world_id}/{event_definition_id}'\n",
        "FOG_DATA_URL = f\"https://forgeofgames-f.azurewebsites.net/api/hoh/inGameData/parse\"\n",
        "\n",
        "PROTOBUF_CONTENT_TYPE = 'application/x-protobuf'\n",
        "JSON_CONTENT_TYPE = 'application/json'\n",
        "GAME_LOGIN_URL = \"https://{subdomain}.heroesgame.com/api/login\"\n",
        "ACCOUNT_PLAY_URL = \"https://{subdomain}.heroesofhistorygame.com/core/api/account/play\"\n",
        "WAKEUP_API_URL = \"https://{world_id}.heroesofhistorygame.com/game/wakeup\"\n",
        "\n",
        "PLAYER_ID_START_CELL = 'A3'\n",
        "PLAYER_NAME_START_CELL = 'B3'\n",
        "EVENT_ID_START_CELL = 'D1'\n",
        "\n",
        "hoh_username = userdata.get('HOH_USERNAME')\n",
        "hoh_password = userdata.get('HOH_PASSWORD')\n",
        "if SPREADSHEET_ID == '':\n",
        "  SPREADSHEET_ID = userdata.get('SPREADSHEET_ID')"
      ],
      "metadata": {
        "id": "S6eg_GNuzVtl"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "@dataclass\n",
        "class PlayerAthPointsDto:\n",
        "    eventId: str\n",
        "    points: str\n",
        "    playerId: int\n",
        "    playerName: str"
      ],
      "metadata": {
        "id": "c19tucACR7Dq"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Common API**"
      ],
      "metadata": {
        "id": "RBjpJLanam4_"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "def to_datetime(d: str):\n",
        "  return datetime.fromisoformat(d)\n",
        "\n",
        "def print_warning(msg: str):\n",
        "  print(f\"\\033[38;5;208mWarning: {msg}\\033[0m\")"
      ],
      "metadata": {
        "id": "M1efyfezUu7e"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Spreadsheet API**"
      ],
      "metadata": {
        "id": "qepnC7-SZ6q6"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "def authenticate_to_google():\n",
        "  auth.authenticate_user()\n",
        "  creds, _ = default()\n",
        "  return gspread.authorize(creds)"
      ],
      "metadata": {
        "id": "aSzdNBeY2C5L"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "metadata": {
        "id": "bd8337b6"
      },
      "source": [
        "def get_next_cell_in_column(cell_coord: str):\n",
        "  coord = parse_cell_coordinate(cell_coord)\n",
        "  next_row_number = coord[1] + 1\n",
        "  next_cell_coord = f\"{coord[0]}{next_row_number}\"\n",
        "\n",
        "  return next_cell_coord"
      ],
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def find_cell_in_row(worksheet: gspread.Worksheet, start_coord: str, value) -> tuple[gspread.Cell, bool]:\n",
        "  coord = parse_cell_coordinate(start_coord)\n",
        "  end_coord = rowcol_to_a1(coord[1], 100)\n",
        "  row_cells = worksheet.range(f'{start_coord}:{end_coord}')\n",
        "\n",
        "  for c in row_cells:\n",
        "    if c.value == str(value):\n",
        "      return (c, True)\n",
        "    elif c.value == '':\n",
        "      return (c, False)\n"
      ],
      "metadata": {
        "id": "Hm46en6mMRbW"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def find_cell_in_col(worksheet: gspread.Worksheet, start_coord: str, value) -> tuple[gspread.Cell, bool]:\n",
        "  coord = parse_cell_coordinate(start_coord)\n",
        "  end_coord = f'{coord[0]}100'\n",
        "  col_cells = worksheet.range(f'{start_coord}:{end_coord}')\n",
        "\n",
        "  for c in col_cells:\n",
        "    if c.value == str(value):\n",
        "      return (c, True)\n",
        "    elif c.value == '':\n",
        "      return (c, False)"
      ],
      "metadata": {
        "id": "WZw8vDuXVqrG"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "metadata": {
        "id": "dcb9ee72"
      },
      "source": [
        "def parse_cell_coordinate(cell_coord: str) -> tuple[str, int] | None:\n",
        "  match = re.match(r\"([A-Z]+)(\\d+)\", cell_coord.upper())\n",
        "  if not match:\n",
        "    print(f\"Invalid cell coordinate format: {cell_coord}\")\n",
        "    return None\n",
        "\n",
        "  column_letter = match.group(1)\n",
        "  row_number = int(match.group(2))\n",
        "\n",
        "  return (column_letter, row_number)"
      ],
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "metadata": {
        "id": "4c0a3231"
      },
      "source": [
        "def autosize_col(worksheet: gspread.Worksheet, col: int):\n",
        "  worksheet.spreadsheet.batch_update({\n",
        "      \"requests\": [\n",
        "          {\n",
        "              \"autoResizeDimensions\": {\n",
        "                  \"dimensions\": {\n",
        "                      \"sheetId\": worksheet.id,\n",
        "                      \"dimension\": \"COLUMNS\",\n",
        "                      \"startIndex\": col - 1,\n",
        "                      \"endIndex\": col\n",
        "                  }\n",
        "              }\n",
        "          }\n",
        "      ]\n",
        "  })\n"
      ],
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Innogames API**"
      ],
      "metadata": {
        "id": "I-1DknA0wlA0"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "def default_headers():\n",
        "    return {\"Content-Type\": JSON_CONTENT_TYPE}\n",
        "\n",
        "def bin_data_headers(session_data):\n",
        "    return {\n",
        "        \"X-AUTH-TOKEN\": session_data[\"sessionId\"],\n",
        "        \"X-Request-Id\": str(uuid.uuid4()),\n",
        "        \"X-Platform\": \"browser\",\n",
        "        \"X-ClientVersion\": session_data[\"clientVersion\"],\n",
        "        \"Accept-Encoding\": \"gzip\",\n",
        "        \"Content-Type\": PROTOBUF_CONTENT_TYPE,\n",
        "        \"Accept\": PROTOBUF_CONTENT_TYPE,\n",
        "    }\n",
        "\n",
        "def login():\n",
        "    session = requests.Session()\n",
        "\n",
        "    payload = {\n",
        "        \"username\": hoh_username,\n",
        "        \"password\": hoh_password,\n",
        "        \"useRememberMe\": False\n",
        "    }\n",
        "\n",
        "    subdomain = 'www'\n",
        "    if GAME_WORLD_ID.startswith('zz'):\n",
        "      subdomain = 'beta'\n",
        "    response = session.post(GAME_LOGIN_URL.format(subdomain = subdomain), headers=default_headers(), json=payload)\n",
        "    response.raise_for_status()\n",
        "    login_data = response.json()\n",
        "\n",
        "    redirect_res = session.get(login_data[\"redirectUrl\"])\n",
        "    redirect_res.raise_for_status()\n",
        "\n",
        "    client_version_match = re.search(r'const\\s+clientVersion\\s*=\\s*\"([^\"]+)\"', redirect_res.text)\n",
        "    if not client_version_match:\n",
        "        raise Exception(\"Client version not found.\")\n",
        "\n",
        "    client_version = client_version_match.group(1)\n",
        "\n",
        "    play_payload = {\n",
        "        \"createDeviceToken\": False,\n",
        "        \"meta\": {\n",
        "            \"clientVersion\": client_version,\n",
        "            \"device\": \"browser\",\n",
        "            \"deviceHardware\": \"browser\",\n",
        "            \"deviceManufacturer\": \"none\",\n",
        "            \"deviceName\": \"browser\",\n",
        "            \"locale\": \"en_DK\",\n",
        "            \"networkType\": \"wlan\",\n",
        "            \"operatingSystemName\": \"browser\",\n",
        "            \"operatingSystemVersion\": \"1\",\n",
        "            \"userAgent\": \"hoh-player-ath-processor\"\n",
        "        },\n",
        "        \"network\": \"BROWSER_SESSION\",\n",
        "        \"token\": \"\",\n",
        "        \"worldId\": None\n",
        "    }\n",
        "\n",
        "    subdomain = 'un0'\n",
        "    if GAME_WORLD_ID.startswith('zz'):\n",
        "      subdomain = 'zz0'\n",
        "    res = session.post(ACCOUNT_PLAY_URL.format(subdomain = subdomain), headers=default_headers(), json=play_payload)\n",
        "    res.raise_for_status()\n",
        "\n",
        "    session_data = res.json()\n",
        "    session_data[\"clientVersion\"] = client_version\n",
        "    return session_data\n",
        "\n",
        "\n",
        "def get_bin_data(url, session_data):\n",
        "    res = requests.post(url, headers=bin_data_headers(session_data))\n",
        "    res.raise_for_status()\n",
        "    return res.content\n",
        "\n",
        "def get_game_data():\n",
        "  session_data = login()\n",
        "  bin_data = get_bin_data(WAKEUP_API_URL.format(world_id = GAME_WORLD_ID), session_data)\n",
        "  return base64.b64encode(bin_data).decode('utf-8')"
      ],
      "metadata": {
        "id": "fBvleQ_ByG7H"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Treasure Hunt API**"
      ],
      "metadata": {
        "id": "9yiRbW-naMWv"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "def get_or_create_worksheet(gc):\n",
        "  spreadsheet = gc.open_by_key(SPREADSHEET_ID)\n",
        "  print(f\"Successfully opened sheet: {spreadsheet.title}\")\n",
        "  try:\n",
        "    worksheet = spreadsheet.worksheet(ALLIANCE_NAME)\n",
        "    print(f\"Worksheet '{ALLIANCE_NAME}' already exists.\")\n",
        "    return worksheet\n",
        "  except gspread.WorksheetNotFound:\n",
        "    print(f\"Worksheet '{ALLIANCE_NAME}' not found. Creating it...\")\n",
        "    worksheet = spreadsheet.add_worksheet(title=ALLIANCE_NAME, rows=\"100\", cols=\"100\")\n",
        "    create_worksheet_base_content(worksheet)\n",
        "    print(f\"Worksheet '{ALLIANCE_NAME}' created successfully.\")\n",
        "    return worksheet"
      ],
      "metadata": {
        "id": "iGPRtGAuFa8Z"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def create_worksheet_base_content(worksheet: gspread.Worksheet):\n",
        "  worksheet.update_acell('C1', 'Event ID')\n",
        "  worksheet.update_acell('A2', 'Player ID')\n",
        "  worksheet.update_acell('B2', 'Player name')\n",
        "  worksheet.update_acell('C2', 'Event dates')\n",
        "\n",
        "  center_format = {\n",
        "      \"horizontalAlignment\": \"CENTER\",\n",
        "      \"verticalAlignment\": \"MIDDLE\"\n",
        "  }\n",
        "\n",
        "  worksheet.format('1:1', center_format)\n",
        "  worksheet.format('2:2', center_format)\n",
        "\n",
        "  grid_range = a1_range_to_grid_range('A1:C2', sheet_id=worksheet.id)\n",
        "  worksheet.spreadsheet.batch_update({\n",
        "      \"requests\": [\n",
        "          {\n",
        "              \"updateSheetProperties\": {\n",
        "                    \"properties\": {\n",
        "                        \"sheetId\": worksheet.id,\n",
        "                        \"gridProperties\": {\n",
        "                            \"frozenRowCount\": 2\n",
        "                        }\n",
        "                    },\n",
        "                    \"fields\": \"gridProperties.frozenRowCount\"\n",
        "                }\n",
        "          },\n",
        "          {\n",
        "              \"updateBorders\": {\n",
        "                \"range\": grid_range,\n",
        "                \"top\":    {\"style\": \"SOLID\", \"width\": 1, \"color\": {\"red\": 0, \"green\": 0, \"blue\": 0}},\n",
        "                \"bottom\": {\"style\": \"SOLID\", \"width\": 1, \"color\": {\"red\": 0, \"green\": 0, \"blue\": 0}},\n",
        "                \"left\":   {\"style\": \"SOLID\", \"width\": 1, \"color\": {\"red\": 0, \"green\": 0, \"blue\": 0}},\n",
        "                \"right\":  {\"style\": \"SOLID\", \"width\": 1, \"color\": {\"red\": 0, \"green\": 0, \"blue\": 0}},\n",
        "            }\n",
        "          }\n",
        "      ]\n",
        "  })"
      ],
      "metadata": {
        "id": "rvN_akniHf33"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "metadata": {
        "id": "adc15ca8"
      },
      "source": [
        "def update_events(worksheet: gspread.Worksheet, events):\n",
        "  sorted_events = sorted(events, key=lambda x: to_datetime(x['startAt']))\n",
        "\n",
        "  for event in sorted_events:\n",
        "    start_date_str = event['startAt']\n",
        "    start_date = to_datetime(start_date_str)\n",
        "    if(start_date < CUTOFF_DATE):\n",
        "      continue\n",
        "\n",
        "    event_id = event['id']\n",
        "    cell_t = find_cell_in_row(worksheet, EVENT_ID_START_CELL, event_id)\n",
        "    if not cell_t[1]:\n",
        "      cell = cell_t[0]\n",
        "      worksheet.update_cell(cell.row, cell.col, event_id)\n",
        "      label_cell_coord = get_next_cell_in_column(cell.address)\n",
        "      end_date_str = event['endAt']\n",
        "      end_date = to_datetime(end_date_str)\n",
        "      formatted_start_date = start_date.strftime('%d/%m/%y')\n",
        "      formatted_end_date = end_date.strftime('%d/%m/%y')\n",
        "      label = f\"{formatted_start_date} - {formatted_end_date}\"\n",
        "      worksheet.update_acell(label_cell_coord, label)\n",
        "      autosize_col(worksheet, cell.col)\n"
      ],
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def update_players(worksheet: gspread.Worksheet, player_rankings: List[PlayerAthPointsDto]):\n",
        "  _, player_name_column = a1_to_rowcol(PLAYER_NAME_START_CELL)\n",
        "  sorted_player_rankings = sorted(player_rankings, key=lambda x: x.playerName)\n",
        "  for pr in sorted_player_rankings:\n",
        "    player_cell, found = find_cell_in_col(worksheet, PLAYER_ID_START_CELL, pr.playerId)\n",
        "    if found:\n",
        "      worksheet.update_cell(player_cell.row, player_name_column, pr.playerName)\n",
        "    else:\n",
        "      worksheet.update_cell(player_cell.row, player_cell.col, pr.playerId)\n",
        "      worksheet.update_cell(player_cell.row, player_name_column, pr.playerName)\n",
        "\n"
      ],
      "metadata": {
        "id": "wW4104t1Wb2j"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def update_rankings(worksheet: gspread.Worksheet, player_rankings: List[PlayerAthPointsDto]):\n",
        "  _, player_name_column = a1_to_rowcol(PLAYER_NAME_START_CELL)\n",
        "  for pr in player_rankings:\n",
        "    event_cell, event_cell_found = find_cell_in_row(worksheet, EVENT_ID_START_CELL, pr.eventId)\n",
        "    if event_cell_found:\n",
        "      player_cell, player_cell_found = find_cell_in_col(worksheet, PLAYER_ID_START_CELL, pr.playerId)\n",
        "      if player_cell_found:\n",
        "        worksheet.update_cell(player_cell.row, event_cell.col, pr.points)\n",
        "      else:\n",
        "        print_warning(f\"Could not find player cell for Player - {pr.playerId}:{pr.playerName}\")\n",
        "    else:\n",
        "      print_warning(f\"Could not find event cell for EventId {pr.eventId}; Player - {pr.playerId}:{pr.playerName}\")\n",
        "\n",
        "\n"
      ],
      "metadata": {
        "id": "q8SarTfYS3_h"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**Forge of Games API**"
      ],
      "metadata": {
        "id": "rWXDregvaFtN"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "def get_ath_events():\n",
        "  url = FOG_API_EVENTS.format(world_id=GAME_WORLD_ID, event_definition_id='TreasureHuntLeague')\n",
        "  res = requests.get(url)\n",
        "  res.raise_for_status()\n",
        "  return res.json()\n"
      ],
      "metadata": {
        "id": "TsRgaW6yw3WE"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "code",
      "source": [
        "def send_data(data: str):\n",
        "  collectionCategoryIds = ['alliance']\n",
        "  if ALLOW_ALLIANCE_ATH_LEADERBOARD_EXPORT:\n",
        "    collectionCategoryIds.append('leaderboards')\n",
        "  payload = {\n",
        "      \"base64ResponseData\": data,\n",
        "      \"responseUrl\": WAKEUP_API_URL.format(world_id = GAME_WORLD_ID),\n",
        "      \"collectionCategoryIds\": collectionCategoryIds\n",
        "  }\n",
        "  res = requests.post(FOG_DATA_URL, headers=default_headers(), json=payload)\n",
        "  res.raise_for_status()\n",
        "  return res.json()\n"
      ],
      "metadata": {
        "id": "QmRdBUAA89yH"
      },
      "execution_count": null,
      "outputs": []
    },
    {
      "cell_type": "markdown",
      "source": [
        "**MAIN**"
      ],
      "metadata": {
        "id": "8N-qE9Pw0CR5"
      }
    },
    {
      "cell_type": "code",
      "source": [
        "gc = authenticate_to_google()\n",
        "ws = get_or_create_worksheet(gc)\n",
        "events = get_ath_events()\n",
        "print('Event list fetched.')\n",
        "update_events(ws, events)\n",
        "print('Event list updated.')\n",
        "data = get_game_data()\n",
        "print('Game data fetched.')\n",
        "parsed_data = send_data(data)\n",
        "print('Game data parsed.')\n",
        "player_rankings = [PlayerAthPointsDto(**item_dict) for item_dict in parsed_data]\n",
        "update_players(ws, player_rankings)\n",
        "print('Players updated.')\n",
        "update_rankings(ws, player_rankings)\n",
        "print('Rankings updated.')\n",
        "\n",
        "print()\n",
        "print('DONE')"
      ],
      "metadata": {
        "id": "fKaEdZY6DRug"
      },
      "execution_count": null,
      "outputs": []
    }
  ]
}