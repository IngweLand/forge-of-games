namespace Ingweland.Fog.HohProtoParser;

public interface IDownloader
{
    Task<DownloadResult> DownloadAsync(string? location);
}
