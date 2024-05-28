using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;

[Table("Commentaries")]
public class CommentaryDbModel(Guid id,
                               Guid authorId,
                               Guid audiotrackId,
                               string text)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [ForeignKey("Audiotrack")]
    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [Required]
    [Column("text", TypeName = "text")]
    public string Text { get; set; } = text;

    [BsonIgnore]
    public UserDbModel? Author { get; set; }
    [BsonIgnore]
    public AudiotrackDbModel? Audiotrack { get; set; }
}