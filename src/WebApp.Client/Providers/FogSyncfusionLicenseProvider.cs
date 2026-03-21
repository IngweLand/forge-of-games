namespace Ingweland.Fog.WebApp.Client.Providers;

public static partial class FogSyncfusionLicenseProvider
{
    public static void Register()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(LicenseKey);
    }
}