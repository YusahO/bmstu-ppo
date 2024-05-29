using MewingPad.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.MongoDB.Models;

[Table("Reports")]
public class ReportDbModel(Guid id,
                           Guid authorId,
                           Guid audiotrackId,
                           string text,
                           ReportStatus status)
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

    [Required]
    [Column("status")]
    public ReportStatus Status { get; set; } = status;
}