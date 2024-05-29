using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.MongoDB.Models;

[Table("Audiotracks")]
[BsonIgnoreExtraElements]
public class AudiotrackDbModel(Guid id,
                               string title,
                               float duration,
                               Guid authorId,
                               string filepath)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [Column("title")]
    public string Title { get; set; } = title;

    [Required]
    [Column("duration")]
    public float Duration { get; set; } = duration;

    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("filepath")]
    public string Filepath { get; set; } = filepath;

    [BsonElement("tag_ids")]
    public List<Guid> TagIds { get; set; } = [];

    [BsonElement("playlist_ids")]
    public List<Guid> PlaylistIds { get; set; } = [];
}