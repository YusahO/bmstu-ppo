using MewingPad.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class ReportDbModel(Guid id,
                           Guid authorId,
                           Guid audiofileId,
                           string text,
                           ReportStatus status)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [ForeignKey("Author")]
    [Column("author_id")]
    public Guid AuthorId { get; set; } = authorId;

    [ForeignKey("Audiofile")]
    [Column("audiofile_id")]
    public Guid AudiofileId { get; set; } = audiofileId;

    [Required]
    [Column("text", TypeName = "text")]
    public string Text { get; set; } = text;

    [Required]
    [Column("status", TypeName = "nvarchar(50)")]
    public ReportStatus Status { get; set; } = status;

    public UserDbModel? Author { get; set; }
    public AudiofileDbModel? Audiofile { get; set; }
}