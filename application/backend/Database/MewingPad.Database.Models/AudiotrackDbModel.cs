using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;

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

    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("filepath")]
    public string Filepath { get; set; } = filepath;

    [BsonIgnore]
    public UserDbModel? Author { get; set; }

    // [BsonIgnore]
    public List<PlaylistDbModel> Playlists { get; } = [];
    [BsonIgnore]
    public List<PlaylistAudiotrackDbModel> PlaylistsAudiotracks { get; set; } = [];

    [BsonIgnore]
    public List<TagDbModel> Tags { get; } = [];
    [BsonIgnore]
    public List<TagAudiotrackDbModel> TagsAudiotracks { get; set; } = [];
}