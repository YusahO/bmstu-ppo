using Newtonsoft.Json;

namespace MewingPad.Common.DTOs;

[JsonObject]
public class UserDto(Guid userId,
                     Guid favouritesId,
                     string username,
                     string email,
                     string passwordHashed,
                     bool isAdmin = false)
{
    [JsonProperty("userId")]
    public Guid Id { get; set; } = userId;

    [JsonProperty("favouritesId")]
    public Guid FavouritesId { get; set; } = favouritesId;

    [JsonProperty("username")]
    public string Username { get; set; } = username;

    [JsonProperty("email")]
    public string Email { get; set; } = email;

    [JsonProperty("password")]
    public string PasswordHashed { get; set; } = passwordHashed;

    [JsonProperty("password")]
    public bool IsAdmin { get; set; } = isAdmin;
}
