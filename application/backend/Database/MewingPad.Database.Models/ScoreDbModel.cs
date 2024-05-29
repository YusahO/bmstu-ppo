using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes; 

namespace MewingPad.Database.Models;

[Table("Scores")]
public class ScoreDbModel(Guid authorId,
                          Guid audiotrackId,
                          int value)
{
    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [ForeignKey("Audiotrack")]
    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [Required]
    [Column("value")]
    public int Value { get; set; } = value;

[BsonIgnore]
    public UserDbModel? Author { get; set; }
[BsonIgnore]
    public AudiotrackDbModel? Audiotrack { get; set; }
}