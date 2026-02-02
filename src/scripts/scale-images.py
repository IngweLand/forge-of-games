#!/usr/bin/env python3
"""
Scale PNG images to fit within a target rectangle while preserving aspect ratio.
Uses high-quality Lanczos resampling for best results with small icons.
"""

import os
import shutil
from pathlib import Path
from PIL import Image


def clear_or_create_dir(target_dir):
    """Remove directory if it exists, then create it fresh."""
    target_path = Path(target_dir)

    if target_path.exists():
        shutil.rmtree(target_path)

    target_path.mkdir(parents=True, exist_ok=True)
    print(f"Created fresh directory: {target_dir}")


def convert_to_greyscale(image):
    """
    Convert image to greyscale while preserving alpha channel if present.
    """
    if image.mode == 'RGBA':
        # Split into RGB and Alpha
        rgb = image.convert('RGB').convert('L')
        alpha = image.split()[3]
        # Merge greyscale with alpha
        greyscale = Image.merge('LA', (rgb, alpha))
    else:
        greyscale = image.convert('L')

    return greyscale


def scale_image_to_fit(image, target_width, target_height):
    """
    Scale image to fit within target dimensions while preserving aspect ratio.
    Uses Lanczos resampling for best quality with small icons.
    """
    original_width, original_height = image.size

    # Calculate scaling factor to fit within target rectangle
    width_ratio = target_width / original_width
    height_ratio = target_height / original_height
    scale_factor = min(width_ratio, height_ratio)

    # Calculate new dimensions
    new_width = int(original_width * scale_factor)
    new_height = int(original_height * scale_factor)

    # Resize using Lanczos (best quality for downscaling)
    scaled_image = image.resize((new_width, new_height), Image.Resampling.LANCZOS)

    return scaled_image


def process_images(src_dir, target_dir, target_width=64, target_height=64, greyscale=False):
    """
        Process all PNG images from source directory and save scaled versions to target.

        Args:
            src_dir: Source directory containing PNG images
            target_dir: Target directory for scaled images
            target_width: Maximum width for scaled images
            target_height: Maximum height for scaled images
            greyscale: If True, convert images to greyscale
    """
    src_path = Path(src_dir)
    target_path = Path(target_dir)

    if not src_path.exists():
        print(f"Error: Source directory '{src_dir}' does not exist!")
        return

    clear_or_create_dir(target_dir)

    png_files = list(src_path.glob("*.png")) + list(src_path.glob("*.PNG"))

    if not png_files:
        print(f"No PNG files found in {src_dir}")
        return

    print(f"\nProcessing {len(png_files)} PNG images...")
    print(f"Target size: {target_width}x{target_height} (aspect ratio preserved)\n")

    success_count = 0
    for png_file in png_files:
        try:
            with Image.open(png_file) as img:
                # Convert to RGBA if not already (to preserve transparency)
                if img.mode != 'RGBA':
                    img = img.convert('RGBA')

                if greyscale:
                    img = convert_to_greyscale(img)

                scaled_img = scale_image_to_fit(img, target_width, target_height)

                output_path = target_path / png_file.name
                scaled_img.save(output_path, 'PNG', optimize=True)

                print(f"✓ {png_file.name}: {img.size} → {scaled_img.size}")
                success_count += 1

        except Exception as e:
            print(f"✗ Error processing {png_file.name}: {e}")

    print(f"\nCompleted: {success_count}/{len(png_files)} images processed successfully")


if __name__ == "__main__":
    import sys

    # Example usage
    if len(sys.argv) < 3:
        print("Usage: python scale_icons.py <source_dir> <target_dir> [width] [height] [--greyscale]")
        print("\nExample:")
        print("  python scale_icons.py ./icons ./scaled_icons 64 64")
        print("  python scale_icons.py ./icons ./scaled_icons 64 64 --greyscale")
        sys.exit(1)

    src = sys.argv[1]
    target = sys.argv[2]
    width = 64
    height = 64
    greyscale = False

    # Parse remaining arguments
    for arg in sys.argv[3:]:
        if arg == '--greyscale' or arg == '--grayscale':
            greyscale = True
        elif width == 64:
            width = int(arg)
        elif height == 64:
            height = int(arg)

    process_images(src, target, width, height, greyscale)