using Microsoft.EntityFrameworkCore; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

[Table("Playlists")]
[Index(nameof(UserId), IsUnique = false)]
public class PlaylistDbModel(Guid id,
                             string title,
                             Guid userId)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [Column("title", TypeName = "varchar(64)")]
    public string Title { get; set; } = title;

    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public Guid UserId { get; set; } = userId;

    public UserDbModel? User { get; set; }

    public List<AudiotrackDbModel> Audiotracks { get; } = [];
    public List<PlaylistAudiotrackDbModel> PlaylistsAudiotracks { get; } = [];
}