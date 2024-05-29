using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace MewingPad.Database.Models;

[Table("PlaylistsAudiotracks")]
[BsonIgnoreExtraElements]
public class PlaylistAudiotrackDbModel(Guid playlistId,
                                       Guid audiotrackId)
{
    [ForeignKey(nameof(Playlist))]
    [Column("playlist_id")]
    public Guid PlaylistId { get; set; } = playlistId;

    [ForeignKey(nameof(Audiotrack))]
    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [BsonIgnore]
    public PlaylistDbModel? Playlist { get; set; }
    [BsonIgnore]
    public AudiotrackDbModel? Audiotrack { get; set; }
}