using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.MongoDB.Models;

[Table("Users")]
public class UserDbModel(Guid id,
                         Guid favouritesId,
                         string username,
                         string passwordHashed,
                         string email,
                         bool isAdmin)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [Column("favourites_id")]
    public Guid FavouritesId { get; set; } = favouritesId;

    [Required]
    [Column("username")]
    public string Username { get; set; } = username;

    [Required]
    [Column("password")]
    public string PasswordHashed { get; set; } = passwordHashed;

    [Required]
    [Column("email")]
    public string Email { get; set; } = email;

    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; } = isAdmin;
}
