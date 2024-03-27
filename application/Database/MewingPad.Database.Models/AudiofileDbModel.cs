using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class AudiofileDbModel(Guid id,
                              string title,
                              float duration,
                              Guid authorId,
                              string filepath)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [Column("title", TypeName = "varchar(64)")]
    public string Title { get; set; } = title;

    [Required]
    [Column("duration", TypeName = "real")]
    public float Duration { get; set; } = duration;

    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("filepath", TypeName = "text")]
    public string Filepath { get; set; } = filepath;

    public UserDbModel? Author { get; set; }

    public List<PlaylistDbModel> Playlists { get; } = [];
    public List<PlaylistAudiofileDbModel> PlaylistsAudiofiles { get; set; } = [];

    public List<TagDbModel> Tags { get; } = [];
    public List<TagAudiofileDbModel> TagsAudiofiles { get; set; } = [];
}