import base64
import gzip
from pathlib import Path


def process_string(input_str: str, output_path: str):
    # 1) decode base64
    step1 = base64.b64decode(input_str)

    # 2) unzip (gzip)
    step2 = gzip.decompress(step1)

    # 3) decode base64 again
    step3 = base64.b64decode(step2)

    # 4) save as bin file
    Path(output_path).write_bytes(step3)


# example usage
process_string(
    "",
    "output.bin")
