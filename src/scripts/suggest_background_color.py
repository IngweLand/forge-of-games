from PIL import Image
import numpy as np
from sklearn.cluster import KMeans
from colorsys import rgb_to_hsv, hsv_to_rgb


def get_complementary_color(color):
    """Get a complementary color by shifting hue by 180 degrees"""
    r, g, b = [x / 255.0 for x in color]
    h, s, v = rgb_to_hsv(r, g, b)
    h = (h + 0.5) % 1.0
    r, g, b = hsv_to_rgb(h, s, v)
    return tuple(int(x * 255) for x in (r, g, b))


def suggest_background_color(image_path):
    """Analyze image and suggest a background color"""
    # Open image and convert to RGBA
    img = Image.open(image_path).convert('RGBA')

    # Convert image to numpy array
    img_array = np.array(img)

    # Get only non-transparent pixels
    non_transparent = img_array[img_array[:, :, 3] > 0]

    if len(non_transparent) == 0:
        return None, "No non-transparent pixels found in the image"

    # Convert to RGB for clustering
    rgb_pixels = non_transparent[:, :3]

    # Use K-means to find dominant colors
    kmeans = KMeans(n_clusters=3, random_state=42)
    kmeans.fit(rgb_pixels)

    # Get the dominant color (most frequent cluster center)
    cluster_sizes = np.bincount(kmeans.labels_)
    dominant_color = tuple(map(int, kmeans.cluster_centers_[np.argmax(cluster_sizes)]))

    # Get complementary color
    background_color = get_complementary_color(dominant_color)

    return background_color, {
        "dominant_color": dominant_color,
        "background_color": background_color
    }


def main():
    # Replace with your image path
    image_path = r'D:\IngweLand\Projects\forge-of-games\assets\hoh\export\videos\frames\WilliamWallace\frame_000000.png'

    try:
        background_color, info = suggest_background_color(image_path)

        if background_color:
            print(f"Analysis complete!")
            print(f"Dominant character color (RGB): {info['dominant_color']}")
            print(f"Suggested background color (RGB): {info['background_color']}")
            print(
                f"Suggested background color (HEX): #{background_color[0]:02x}{background_color[1]:02x}{background_color[2]:02x}")
        else:
            print("Could not analyze the image. Make sure it contains non-transparent pixels.")

    except Exception as e:
        print(f"Error processing image: {str(e)}")


if __name__ == "__main__":
    main()