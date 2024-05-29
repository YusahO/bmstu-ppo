using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.MongoDB.Models;

[Table("Scores")]
public class ScoreDbModel(Guid authorId,
                          Guid audiotrackId,
                          int value)
{
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [Required]
    [Column("value")]
    public int Value { get; set; } = value;
}