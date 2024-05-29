using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.MongoDB.Models;

[Table("Tags")]
public class TagDbModel(Guid id,
                        Guid authorId,
                        string name)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("name")]
    public string Name { get; set; } = name;

    [Column("audiotrack_ids")]
    public List<Guid> AudiotrackIds { get; set; } = [];
}