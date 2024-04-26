﻿using Newtonsoft.Json;

namespace MewingPad.UI.DTOs;

[JsonObject]
public class UserDto(Guid id,
                     Guid favouritesId,
                     string username,
                     string email,
                     string passwordHashed,
                     bool isAdmin = false)
{
    [JsonProperty("userId")]
    public Guid Id { get; set; } = id;

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