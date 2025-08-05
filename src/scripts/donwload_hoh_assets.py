import os
import shutil
from concurrent.futures import ThreadPoolExecutor, as_completed
from typing import List
from urllib.parse import urlparse

import requests

download_dir = r'D:\IngweLand\Projects\forge-of-games-resources\hoh\assets\downloads'
data_file = r'D:\IngweLand\Projects\forge-of-games-resources\hoh\assets\catalog.bin'
cdn_root = 'https://heczz.innogamescdn.com/bundles/WebGL/'
data_url_prefix = '00 00 00'
end_marker = '.bundle'
files_to_skip = ['vfx', 'pfx']
files_to_download = []


def parse_binary_file(file_path) -> List[str]:
    search_bytes = bytes.fromhex(data_url_prefix)  # Start marker
    end_bytes = end_marker.encode('utf-8')          # End marker

    with open(file_path, 'rb') as file:
        data = file.read()

    results = []
    search_bytes_len = len(search_bytes)
    end_bytes_len = len(end_bytes)
    search_start = 0

    while (end_pos := data.find(end_bytes, search_start)) != -1:
        # Search backward for start marker from just before the end marker
        start_pos = data.rfind(search_bytes, 0, end_pos)
        if start_pos != -1:
            # Extract and decode the segment between the markers
            content_start = start_pos + search_bytes_len
            content_end = end_pos + end_bytes_len
            url = cdn_root + data[content_start:content_end].decode('utf-8', errors='ignore')
            if should_download(url):
                results.append(url)
        # Move past this end marker for the next iteration
        search_start = end_pos + end_bytes_len

    return results


def should_download(url: str) -> bool:
    parsed_url = urlparse(url)
    filename = parsed_url.path[parsed_url.path.rfind('/') + 1:]
    if len(files_to_download) > 0:
        for download_str in files_to_download:
            if download_str in filename:
                return True
        return False
    for skip_str in files_to_skip:
        if filename.startswith(skip_str):
            return False
    return True


def download_file(url):
    parsed_url = urlparse(url)
    filename = parsed_url.path[parsed_url.path.rfind('/') + 1:]
    save_path = os.path.join(download_dir, filename)
    headers = {
        'User-Agent': 'Mozilla/5.0',
        'Accept-Encoding': 'gzip'
    }
    try:
        response = requests.get(url, stream=True, headers=headers)
        if response.status_code == 200:
            with open(save_path, 'wb') as file:
                for chunk in response.iter_content(chunk_size=8192):
                    file.write(chunk)
            return f"Downloaded: {url}"
        else:
            return f"Failed to download: {url}"
    except Exception as e:
        return f"Error downloading {url}: {str(e)}"


def process_urls(urls, max_workers=10):
    failed_downloads = []
    with ThreadPoolExecutor(max_workers=max_workers) as executor:
        future_to_url = {executor.submit(download_file, url): url for url in urls}
        for future in as_completed(future_to_url):
            result = future.result()
            if result.startswith("Downloaded"):
                print(future.result())
            else:
                failed_downloads.append(result)
    for failed_url in failed_downloads:
        result = download_file(failed_url)
        print(result)


def reset_directories():
    if os.path.exists(download_dir):
        shutil.rmtree(download_dir)
    os.mkdir(download_dir)


def main():
    reset_directories()
    urls = parse_binary_file(data_file)
    print(f"Found {len(urls)} urls")
    process_urls(urls)


if __name__ == "__main__":
    main()
