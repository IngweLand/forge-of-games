using System.IO.Compression;
using System.Text;

namespace Ingweland.Fog.Shared.Utils;

public static class FileUtils
{
    public static async Task<string> UnzipAsync(string path)
    {
        var bytes = await UnzipToBytesInternalAsync(path);
        return Encoding.UTF8.GetString(bytes);
    }

    public static Task<byte[]> UnzipToBytesAsync(string path) =>
        UnzipToBytesInternalAsync(path);

    public static Task<byte[]> UnzipToBytesAsync(Stream stream) =>
        UnzipToBytesInternalAsync(stream);

    private static async Task<byte[]> UnzipToBytesInternalAsync(string path)
    {
        await using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        return await UnzipToBytesInternalAsync(fs);
    }

    private static async Task<byte[]> UnzipToBytesInternalAsync(Stream inputStream)
    {
        try
        {
            await using MemoryStream ms = new();
            await using GZipStream gs = new(inputStream, CompressionMode.Decompress);
            await gs.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            throw new IOException("Error during decompression", ex);
        }
    }

    public static async Task ZipAndSaveAsync(string path, byte[] data)
    {
        try
        {
            await using FileStream fs = new(path, FileMode.Create);
            await using GZipStream gs = new(fs, CompressionMode.Compress);
            await gs.WriteAsync(data);
            await gs.FlushAsync();
        }
        catch (Exception ex)
        {
            throw new IOException($"Error while compressing and saving to {path}", ex);
        }
    }

    public static Task ZipAndSaveAsync(string path, string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        return ZipAndSaveAsync(path, bytes);
    }

    public static async Task<byte[]> ZipAsync(byte[] data)
    {
        try
        {
            await using MemoryStream ms = new();
            await using GZipStream gs = new(ms, CompressionMode.Compress);
            await gs.WriteAsync(data);
            await gs.FlushAsync();
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            throw new IOException("Error during compression", ex);
        }
    }
}
