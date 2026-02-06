from PIL import Image
import numpy as np


def tint_image(image_path, tint_color, output_path=None):
    """
    Tint a grayscale PNG while preserving transparency.

    Args:
        image_path: Path to the grayscale PNG
        tint_color: Tuple of (R, G, B) values (0-255)
        output_path: Where to save (optional)

    Returns:
        PIL Image object
    """
    # Load image with alpha channel
    img = Image.open(image_path).convert('RGBA')
    img_array = np.array(img, dtype=np.float32)

    # Split channels
    r, g, b, a = img_array[:, :, 0], img_array[:, :, 1], img_array[:, :, 2], img_array[:, :, 3]

    # Convert to grayscale (if not already)
    # Use luminosity formula
    gray = 0.299 * r + 0.587 * g + 0.114 * b

    # Normalize grayscale to 0-1
    gray_normalized = gray / 255.0

    # Apply tint color
    tinted = np.zeros_like(img_array)
    tinted[:, :, 0] = gray_normalized * tint_color[0]  # R
    tinted[:, :, 1] = gray_normalized * tint_color[1]  # G
    tinted[:, :, 2] = gray_normalized * tint_color[2]  # B
    tinted[:, :, 3] = a  # Preserve original alpha

    # Convert back to uint8
    tinted = np.clip(tinted, 0, 255).astype(np.uint8)

    # Create image
    result = Image.fromarray(tinted, 'RGBA')

    if output_path:
        result.save(output_path)

    return result


def tint_with_pattern(template_path, pattern_path, output_path=None):
    """
    Apply a pattern image to a grayscale template, preserving shading and transparency.
    Pattern is scaled to cover the template while maintaining aspect ratio.

    Args:
        template_path: Path to the grayscale PNG template (with alpha)
        pattern_path: Path to the pattern image to sample colors from
        output_path: Where to save (optional)

    Returns:
        PIL Image object
    """
    # Load template with alpha
    template = Image.open(template_path).convert('RGBA')
    template_array = np.array(template, dtype=np.float32)

    # Load pattern image
    pattern = Image.open(pattern_path).convert('RGB')

    # Scale pattern to cover template while preserving aspect ratio
    pattern_scaled = scale_to_cover(pattern, template.size)
    pattern_array = np.array(pattern_scaled, dtype=np.float32)

    # Get grayscale values from template
    r, g, b, a = template_array[:, :, 0], template_array[:, :, 1], template_array[:, :, 2], template_array[:, :, 3]
    gray = 0.299 * r + 0.587 * g + 0.114 * b

    # Normalize to 0-1 (this is the brightness multiplier)
    intensity = gray / 255.0

    # Apply pattern colors modulated by template intensity
    result = np.zeros_like(template_array)
    result[:, :, 0] = pattern_array[:, :, 0] * intensity  # R channel
    result[:, :, 1] = pattern_array[:, :, 1] * intensity  # G channel
    result[:, :, 2] = pattern_array[:, :, 2] * intensity  # B channel
    result[:, :, 3] = a  # Preserve original alpha channel

    # Convert back to uint8
    result = np.clip(result, 0, 255).astype(np.uint8)

    # Create image
    result_img = Image.fromarray(result, 'RGBA')

    if output_path:
        result_img.save(output_path)

    return result_img


def scale_to_cover(image, target_size):
    """
    Scale image to cover target size while preserving aspect ratio.
    Crops the image if necessary to fill the entire target.
    """
    target_width, target_height = target_size
    img_width, img_height = image.size

    # Calculate scaling factor to cover target (max instead of min)
    scale = max(target_width / img_width, target_height / img_height)

    # Calculate new size
    new_width = int(img_width * scale)
    new_height = int(img_height * scale)

    # Resize image
    scaled = image.resize((new_width, new_height), Image.LANCZOS)

    # Calculate crop box to center the image
    left = (new_width - target_width) // 2
    top = (new_height - target_height) // 2
    right = left + target_width
    bottom = top + target_height

    # Crop to exact target size
    cropped = scaled.crop((left, top, right, bottom))

    return cropped

def get_bg_list():
    prefix = "Shape"
    suffix = ".png"
    n = 30
    return [f"{prefix}{i:02d}{suffix}" for i in range(1, n+1)]

def get_icon_list():
    prefix = "icon_alliance_symbol_"
    suffix = ".png"
    n = 100
    return [f"{prefix}{i}{suffix}" for i in range(n)]

# Usage:
result = tint_with_pattern('Shape15.png', 'alliance_rainbow_box.png', 'output.png')

# Example usage:
# tinted = tint_image('Shape15.png', (50, 50, 50), 'output.png')

background_colors = ["#9e16cb","#d52595","#dc2933","#e1760d","#fbc800","#404040","#8c8c8c","#73562a","#8cb800","#00ac36","#0d6d64","#007ad4","#2a33d1"]
icon_colors = ["#a8d5eb","#c0eeaa","#efb6ab","#efdeab","#000000","#c8c8c8","#fbfbfb"]
icons = get_icon_list()
backgrounds = get_bg_list()

