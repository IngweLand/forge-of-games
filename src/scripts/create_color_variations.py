from PIL import Image
import os


def create_color_variations(input_dir, color_dict):
    # Get all PNG files
    png_files = [f for f in os.listdir(input_dir) if f.lower().endswith('.png')]

    for image_file in png_files:
        # Load image
        image_path = os.path.join(input_dir, image_file)
        img = Image.open(image_path).convert('RGBA')

        # For each color in dictionary
        for color_name, hex_color in color_dict.items():
            # Convert hex to RGB
            rgb_color = tuple(int(hex_color.lstrip('#')[i:i + 2], 16) for i in (0, 2, 4))

            # Create new image
            new_img = Image.new('RGBA', img.size)

            # Process pixels
            for x in range(img.width):
                for y in range(img.height):
                    r, g, b, a = img.getpixel((x, y))
                    if a > 0:  # Non-transparent pixel
                        new_color = (*rgb_color, a)
                        new_img.putpixel((x, y), new_color)

            # Save new version
            filename, ext = os.path.splitext(image_file)
            new_path = os.path.join(input_dir, f"{filename}_{color_name}{ext}")
            new_img.save(new_path)


# Example usage
colors = {
    "red": "#FF4B32",
    "blue": "#479AFF",
    "green": "#35CF2E",
    "yellow": "#FFAA00",
    "purple": "#7C3BE8",
    "neutral": "#34495E",
}

create_color_variations(r"D:\Temp\New folder", colors)