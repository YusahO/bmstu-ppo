using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.PgSQL.Models;

[Table("Tags")]
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
    [Column("name")]
    public string Name { get; set; } = name;

    public UserDbModel? Author { get; set; }

    public List<AudiotrackDbModel> Audiotracks { get; } = [];
    public List<TagAudiotrackDbModel> TagsAudiotracks { get; set; } = [];
}