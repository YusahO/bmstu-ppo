using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class TagAudiofileDbModel(Guid tagId,
                                 Guid audiofileId)
{
    [ForeignKey("Tag")]
    [Column("tag_id")]
    public Guid TagId { get; set; } = tagId;

    [ForeignKey("Audiofile")]
    [Column("audiofile_id")]
    public Guid AudiofileId { get; set; } = audiofileId;

    public TagDbModel? Tag { get; set; }
    public AudiofileDbModel? Audiofile { get; set; }
}