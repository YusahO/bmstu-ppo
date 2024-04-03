using MewingPad.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

[Table("Reports")]
public class ReportDbModel(Guid id,
                           Guid authorId,
                           Guid audiotrackId,
                           string text,
                           ReportStatus status)
{
    [Key]
    [Column("id")]
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

    [Required]
    [Column("status", TypeName = "varchar(50)")]
    public ReportStatus Status { get; set; } = status;

    public UserDbModel? Author { get; set; }
    public AudiotrackDbModel? Audiotrack { get; set; }
}