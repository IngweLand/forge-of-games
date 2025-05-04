import re
import sys

def process_file(input_file, output_file):
    # Read the input file
    with open(input_file, 'r') as f:
        content = f.read()

    # Find all strings matching the pattern
    pattern = r'type\.googleapis\.com/(\w+)'
    matches = re.findall(pattern, content)

    # Sort the matches alphabetically
    sorted_matches = sorted(set(matches))

    # Write the sorted matches to the output file
    with open(output_file, 'w') as f:
        for match in sorted_matches:
            f.write(match + '\n')

if __name__ == "__main__":
    input_file = r'D:\IngweLand\Projects\forge-of-games\resources\hoh\data\startup_decompiled_proto.txt'
    output_file = 'output.txt'

    process_file(input_file, output_file)
    print(f"Processing complete. Results saved to {output_file}")