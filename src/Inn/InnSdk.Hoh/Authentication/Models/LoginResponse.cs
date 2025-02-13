namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record LoginResponse(
    int PlayerId,
    string RedirectUrl,
    object Errors
);
