using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

public class TagDbModel(Guid id,
                        Guid authorId,
                        string name)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = id;

    [ForeignKey("Author")]
    public Guid AuthorId { get; set; } = authorId;

    [Required]
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; } = name;

    public UserDbModel? Author { get; set; }

    public List<AudiofileDbModel> Audiofiles { get; } = [];
    public List<TagAudiofileDbModel> TagsAudiofiles { get; set; } = [];
}