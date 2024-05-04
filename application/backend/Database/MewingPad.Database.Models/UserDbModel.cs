using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

[Index(nameof(FavouritesId), IsUnique = true)]
[Table("Users")]
public class UserDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [ForeignKey("FavouritesId")]
    [Column("favourties_id")]
    public Guid FavouritesId { get; set; }

    [Required]
    [Column("username", TypeName = "varchar(64)")]
    public string Username { get; set; }

    [Required]
    [Column("password", TypeName = "varchar(128)")]
    public string PasswordHashed { get; set; }

    [Required]
    [Column("email", TypeName = "varchar(320)")]
    public string Email { get; set; }

    [Required]
    [Column("is_admin", TypeName = "bool")]
    public bool IsAdmin { get; set; }

    public ICollection<PlaylistDbModel> Playlists { get; set; } = [];
    public ICollection<ScoreDbModel> Scores { get; set; } = [];
    public PlaylistDbModel? FavouritesPlaylist { get; set; }

    public UserDbModel(Guid id,
                             Guid favouritesId,
                             string username,
                             string passwordHashed,
                             string email,
                             bool isAdmin)
    {
        Id = id;
        FavouritesId = favouritesId;
        Username = username;
        PasswordHashed = passwordHashed;
        Email = email;
        IsAdmin = isAdmin;
    }
}
