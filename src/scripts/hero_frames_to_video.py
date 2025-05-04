import os
import subprocess
import argparse
from pathlib import Path


def process_animations(input_base_path, output_base_path, ffmpeg_path="ffmpeg",
                       framerate=60, size="1920x1080", bg_color="white", preset="slow", crf=17):
    """
    Process all animation folders into videos.

    Args:
        input_base_path (str): Path to folders containing frame sequences
        output_base_path (str): Path for output videos
        ffmpeg_path (str): Path to ffmpeg executable
        framerate (int): Output video framerate
        bg_color (str): Background color for videos
        preset (str): FFmpeg encoding preset
        crf (int): FFmpeg CRF value
    """
    # Create output directory if it doesn't exist
    Path(output_base_path).mkdir(parents=True, exist_ok=True)

    # Get all directories in the input path
    input_dirs = [d for d in Path(input_base_path).iterdir() if d.is_dir()]

    if not input_dirs:
        print(f"No directories found in {input_base_path}")
        return

    print(f"Found {len(input_dirs)} directories to process")

    for dir_path in input_dirs:
        folder_name = dir_path.name
        input_path = str(dir_path / "frame_%06d.png")
        output_path = str(Path(output_base_path) / f"{folder_name}.mp4")

        print(f"\nProcessing folder: {folder_name}")
        print(f"Input path: {input_path}")
        print(f"Output path: {output_path}")

        # FFmpeg command
        ffmpeg_command = [
            ffmpeg_path,
            "-framerate", str(framerate),
            "-i", input_path,
            "-f", "lavfi",
            "-i", f"color=c={bg_color}:r={str(framerate)}:s={size}",
            "-filter_complex", "[1:v][0:v]overlay=shortest=1",
            "-c:v", "libx264",
            "-pix_fmt", "yuv420p",
            "-preset", preset,
            "-crf", str(crf),
            output_path
        ]

        # Execute the command
        print("Executing FFmpeg command...")
        try:
            subprocess.run(ffmpeg_command, check=True)
            print(f"Successfully processed {folder_name}")
        except subprocess.CalledProcessError as e:
            print(f"Error processing {folder_name}: {e}")
        except FileNotFoundError:
            print(f"FFmpeg not found at: {ffmpeg_path}")
            return


def parse_arguments():
    """Parse command line arguments."""
    parser = argparse.ArgumentParser(description='Process animation frames into videos.')

    parser.add_argument('-i', '--input',
                        default='./input',
                        help='Input base path containing animation folders (default: ./input)')

    parser.add_argument('-o', '--output',
                        default='./output',
                        help='Output path for rendered videos (default: ./output)')

    parser.add_argument('-f', '--ffmpeg',
                        default='ffmpeg',
                        help='Path to FFmpeg executable (default: ffmpeg)')

    parser.add_argument('-r', '--framerate',
                        type=int,
                        default=60,
                        help='Output video framerate (default: 60)')

    parser.add_argument('-s', '--size',
                        default='1920x1080',
                        help='Output video size (default: 1920x1080)')

    parser.add_argument('-b', '--bgcolor',
                        default='black',
                        help='Background color (default: black)')

    parser.add_argument('-p', '--preset',
                        default='veryslow',
                        choices=['ultrafast', 'superfast', 'veryfast', 'faster', 'fast', 'medium', 'slow', 'slower',
                                 'veryslow'],
                        help='FFmpeg encoding preset (default: veryslow)')

    parser.add_argument('-c', '--crf',
                        type=int,
                        default=17,
                        help='FFmpeg CRF value (0-51, lower is better quality) (default: 17)')

    return parser.parse_args()


def main():
    """Main entry point of the script."""
    args = parse_arguments()

    print("Animation Processing Script")
    print("-" * 30)
    print(f"Input path: {args.input}")
    print(f"Output path: {args.output}")
    print(f"FFmpeg path: {args.ffmpeg}")
    print(f"Framerate: {args.framerate}")
    print(f"Background color: {args.bgcolor}")
    print(f"Encoding preset: {args.preset}")
    print(f"CRF value: {args.crf}")
    print("-" * 30)

    try:
        process_animations(
            input_base_path=args.input,
            output_base_path=args.output,
            ffmpeg_path=args.ffmpeg,
            framerate=args.framerate,
            size=args.size,
            bg_color=args.bgcolor,
            preset=args.preset,
            crf=args.crf
        )
        print("\nAll folders processed!")
    except Exception as e:
        print(f"\nAn error occurred: {e}")
        return 1

    return 0


if __name__ == "__main__":
    exit(main())