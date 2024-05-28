using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;

[Table("Tags")]
public class TagDbModel(Guid id,
                        Guid authorId,
                        string name)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [ForeignKey("Author")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; } = name;

    [BsonIgnore]
    public UserDbModel? Author { get; set; }

    [BsonIgnore]
    public List<AudiotrackDbModel> Audiotracks { get; } = [];
    [BsonIgnore]
    public List<TagAudiotrackDbModel> TagsAudiotracks { get; set; } = [];
}