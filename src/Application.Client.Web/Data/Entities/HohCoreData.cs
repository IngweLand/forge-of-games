namespace Ingweland.Fog.Application.Client.Web.Data.Entities;

public class HohCoreData
{
    public required byte[] Data { get; set; }
    public required string Id { get; set; }

    public static class Keys
    {
        public const string ID = "id";
    }
}
