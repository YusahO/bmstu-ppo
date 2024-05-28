using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MewingPad.Database.Models;

namespace MewingPad.Database.Models;


[Table("TagsAudiotracks")]
public class TagAudiotrackDbModel(Guid tagId,
                                  Guid audiotrackId)
{
    [ForeignKey("Tag")]
    [Column("tag_id")]
    [BsonId]
    public Guid TagId { get; set; } = tagId;

    [ForeignKey("Audiotrack")]
    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    public TagDbModel? Tag { get; set; }
    public AudiotrackDbModel? Audiotrack { get; set; }
}