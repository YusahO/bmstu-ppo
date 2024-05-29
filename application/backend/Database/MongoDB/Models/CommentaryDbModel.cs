using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.MongoDB.Models;

[Table("Commentaries")]
[BsonIgnoreExtraElements]
public class CommentaryDbModel(Guid id,
                               Guid authorId,
                               Guid audiotrackId,
                               string text)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [Required]
    [Column("text")]
    public string Text { get; set; } = text;
}