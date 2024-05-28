using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;

[Table("Playlists")]
[Index(nameof(UserId), IsUnique = false)]
public class PlaylistDbModel(Guid id,
                             string title,
                             Guid userId)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [Column("title", TypeName = "varchar(64)")]
    public string Title { get; set; } = title;

    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public Guid UserId { get; set; } = userId;

    [BsonIgnore]
    public UserDbModel? User { get; set; }

    [BsonIgnore]
    public List<AudiotrackDbModel> Audiotracks { get; } = [];
    [BsonIgnore]
    public List<PlaylistAudiotrackDbModel> PlaylistsAudiotracks { get; } = [];
}