using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

[Index(nameof(FavouritesId), IsUnique = true)]
[Table("Users")]
public class UserDbModel(Guid id,
                         Guid favouritesId,
                         string username,
                         string passwordHashed,
                         string email,
                         bool isAdmin,
                         bool isAuthorized)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [ForeignKey("FavouritesId")]
    [Column("favourties_id")]
    public Guid FavouritesId { get; set; } = favouritesId;

    [Required]
    [Column("username", TypeName = "varchar(64)")]
    public string Username { get; set; } = username;

    [Required]
    [Column("password", TypeName = "varchar(128)")]
    public string PasswordHashed { get; set; } = passwordHashed;

    [Required]
    [Column("email", TypeName = "varchar(320)")]
    public string Email { get; set; } = email;

    [Required]
    [Column("is_admin", TypeName = "bool")]
    public bool IsAdmin { get; set; } = isAdmin;

    [Required]
    [Column("is_authorized", TypeName = "bool")]
    public bool IsAuthorized { get; set; } = isAuthorized;

    public ICollection<PlaylistDbModel> Playlists { get; set; } = [];
    public PlaylistDbModel? FavouritesPlaylist { get; set; }
}
