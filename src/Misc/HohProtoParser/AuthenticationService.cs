using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.HohProtoParser;

public class AuthenticationService(
    HttpClient httpClient,
    IOptions<HohCredentials> credentials) : IAuthenticationService
{
    private const string LOGIN_URL = "https://beta.heroesgame.com/api/login";
    private const string ACCOUNT_URL = "https://zz0.heroesofhistorygame.com/core/api/account/play";

    public async Task<AuthResponse> Authenticate()
    {
        //login
        var response = await httpClient.PostAsJsonAsync(LOGIN_URL,
            new LoginRequest(credentials.Value.Username, credentials.Value.Password, false));
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        //redirect
        var redirectedPage = await httpClient.GetStringAsync(loginResponse!.RedirectUrl);
        string pattern = @"const\s+clientVersion\s*=\s*""([^""]+)"";";
        var match = Regex.Match(redirectedPage, pattern);

        string clientVersion;
        if (match.Success)
        {
            clientVersion = match.Groups[1].Value;
        }
        else
        {
            throw new Exception("Game version not found");
        }

        //get auth token
        var accountRawResponse = await httpClient.PostAsJsonAsync(ACCOUNT_URL, new AccountRequest()
        {
            Meta = new DeviceMeta(){ClientVersion = clientVersion},
        });
        accountRawResponse.EnsureSuccessStatusCode();
        var gameAccount =  await accountRawResponse.Content.ReadFromJsonAsync<GameAccount>();
        return new AuthResponse()
        {
            SessionId = gameAccount.SessionId,
            ClientVersion = clientVersion,
        };
    }
}

public record LoginRequest(
    string Username,
    string Password,
    bool UseRememberMe
);

public record LoginResponse(
    int PlayerId,
    string RedirectUrl,
    object Errors
);

public record AccountRequest
{
    public string Network { get; init; } = "BROWSER_SESSION";
    public string Token { get; init; } = string.Empty;
    public string? WorldId { get; init; }
    public bool CreateDeviceToken { get; init; }
    public DeviceMeta Meta { get; init; } = new();
}

public record DeviceMeta
{
    public string ClientVersion { get; init; } = string.Empty;
    public string DeviceHardware { get; init; } = "browser";
    public string DeviceName { get; init; } = "browser";
    public string Locale { get; init; } = "en_DK";
    public string NetworkType { get; init; } = "wlan";
    public string OperatingSystemName { get; init; } = "browser";
    public string OperatingSystemVersion { get; init; } = "131.0.0.0";
    public string? Platform { get; init; }

    public string UserAgent { get; init; } =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";

    public string DeviceManufacturer { get; init; } = "Microsoft";
    public string Device { get; init; } = "Microsoft browser";
}

public record GameAccount
{
    public string GameId { get; init; }
    public string MarketId { get; init; }
    public int WorldId { get; init; }
    public int AccountId { get; init; }
    public string Name { get; init; }
    public string? Nickname { get; init; }
    public string? Password { get; init; }
    public bool Guest { get; init; }
    public bool FirstLogin { get; init; }
    public string? CdnUrl { get; init; }
    public string Url { get; init; }
    public string SessionId { get; init; }
    public string MasterLoginToken { get; init; }
    public string? DeviceToken { get; init; }
    public PrivacySettings PrivacySettings { get; init; }
    public IReadOnlyList<string> SocialLogins { get; init; }
    public string AnonymizedMail { get; init; }
    public bool ConfirmedMail { get; init; }
    public bool RealMailAddress { get; init; }
    public object? TransparencyAndConsent { get; init; }
    public Dictionary<string, object> ClientAttributes { get; init; }
}

public record PrivacySettings
{
    public bool Accepted3rdPartyAds { get; init; }
    public bool Accepted3rdPartyPixels { get; init; }
    public bool AcceptedAdjustEvents { get; init; }
    public bool AcceptedEmailsOption { get; init; }
}
