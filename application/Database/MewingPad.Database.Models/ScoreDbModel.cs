using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class ScoreDbModel(Guid authorId,
                          Guid audiofileId,
                          int value)
{
    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [ForeignKey("Audiofile")]
    [Column("audiofile_id")]
    public Guid AudiofileId { get; set; } = audiofileId;

    [Required]
    [Column("value", TypeName = "integer")]
    public int Value { get; set; } = value;

    public UserDbModel? Author { get; set; }
    public AudiofileDbModel? Audiofile { get; set; }
}