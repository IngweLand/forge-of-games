namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record LoginRequest(
    string Username,
    string Password,
    bool UseRememberMe
);
