using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;


[Table("TagsAudiotracks")]
[BsonIgnoreExtraElements]
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

    [BsonIgnore]
    public TagDbModel? Tag { get; set; }
    [BsonIgnore]
    public AudiotrackDbModel? Audiotrack { get; set; }
}