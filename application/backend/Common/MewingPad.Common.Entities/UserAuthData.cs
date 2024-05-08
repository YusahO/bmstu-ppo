namespace MewingPad.Common.Entities;

public class TokensData
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

public class UserAuthData
{
    public User? User { get; set; }
    public TokensData? TokensData { get; set; }
}