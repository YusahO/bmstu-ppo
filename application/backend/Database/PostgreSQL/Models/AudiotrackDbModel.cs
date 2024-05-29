using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.PgSQL.Models;

[Table("Audiotracks")]
public class AudiotrackDbModel(Guid id,
                               string title,
                               float duration,
                               Guid authorId,
                               string filepath)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [Column("title")]
    public string Title { get; set; } = title;

    [Required]
    [Column("duration")]
    public float Duration { get; set; } = duration;

    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("filepath")]
    public string Filepath { get; set; } = filepath;

    public UserDbModel? Author { get; set; }

    public List<PlaylistDbModel> Playlists { get; } = [];
    public List<PlaylistAudiotrackDbModel> PlaylistsAudiotracks { get; set; } = [];

    public List<TagDbModel> Tags { get; } = [];
    public List<TagAudiotrackDbModel> TagsAudiotracks { get; set; } = [];
}