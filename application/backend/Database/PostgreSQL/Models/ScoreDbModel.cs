using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.PgSQL.Models;

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

    public UserDbModel? Author { get; set; }
    public AudiotrackDbModel? Audiotrack { get; set; }
}