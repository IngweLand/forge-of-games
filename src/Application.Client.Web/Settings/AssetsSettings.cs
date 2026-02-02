namespace Ingweland.Fog.Application.Client.Web.Settings;

public class AssetsSettings
{
    public const string CONFIGURATION_PROPERTY_NAME = "AssetsSettings";
    public required string BaseUrl { get; set; }
    public required string Fonts { get; set; } = "fonts";
    public required string HohIconAtlasHash { get; set; }
    public required string HohIconsPath { get; set; } = "images/hoh/icons";
    public required string HohImagesBasePath { get; set; } = "images/hoh";
    public required string HohPlayerAvatarsPath { get; set; } = "images/hoh/avatars";
    public required string HohUnitImagesPath { get; set; } = "images/hoh/units";
    public required string HohUnitVideosPath { get; set; } = "videos/hoh/units";
    public required string HohVideosBasePath { get; set; } = "videos/hoh";
    public required string ImagesBasePath { get; set; } = "images";
    public required string Version { get; set; } = "1";
}
