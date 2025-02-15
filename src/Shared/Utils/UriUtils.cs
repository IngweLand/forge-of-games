namespace Ingweland.Fog.Shared.Utils;

public static class UriUtils
{
    public static string GetSubdomain(string src)
    {
        var uri = new Uri(src);

        var host = uri.Host;
        var hostParts = host.Split('.');

        return hostParts.Length > 2 ? hostParts[0] : string.Empty;
    }

    public static string GetPath(string src)
    {
        var uri = new Uri(src);

        return uri.AbsolutePath.Trim('/');
    }
}