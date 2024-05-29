using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.PgSQL.Models;

[Index(nameof(FavouritesId), IsUnique = true)]
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
    public Guid Id { get; set; } = id;

    [ForeignKey("FavouritesId")]
    [Column("favourties_id")]
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

    public ICollection<PlaylistDbModel> Playlists { get; set; } = [];
    public ICollection<ScoreDbModel> Scores { get; set; } = [];
    public PlaylistDbModel? FavouritesPlaylist { get; set; }
}
