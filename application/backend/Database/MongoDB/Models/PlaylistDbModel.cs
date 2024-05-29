using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace MewingPad.Database.MongoDB.Models;

[Collection("Playlists")]
[Index(nameof(UserId), IsUnique = false)]
public class PlaylistDbModel(Guid id,
                             string title,
                             Guid userId)
{
    [Key]
    [Column("id")]
    [BsonId]
    public Guid Id { get; set; } = id;

    [Column("title")]
    public string Title { get; set; } = title;

    [Column("user_id")]
    public Guid UserId { get; set; } = userId;

    [Column("audiotrack_ids")]
    public List<Guid> AudiotrackIds { get; set; } = [];
}