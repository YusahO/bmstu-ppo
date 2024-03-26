using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class PlaylistDbModel(Guid id,
                             string title,
                             Guid userId)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [Column("title", TypeName = "varchar(64)")]
    public string Title { get; set; } = title;

    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; } = userId;

    public UserDbModel? User { get; set; }

    public List<AudiofileDbModel> Audiofiles { get; } = [];
    public List<PlaylistAudiofileDbModel> PlaylistsAudiofiles { get; } = [];
}