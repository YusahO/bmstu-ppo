using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.PgSQL.Models;

[Table("PlaylistsAudiotracks")]
public class PlaylistAudiotrackDbModel(Guid playlistId,
                                       Guid audiotrackId)
{
    [ForeignKey(nameof(Playlist))]
    [Column("playlist_id")]
    public Guid PlaylistId { get; set; } = playlistId;

    [ForeignKey(nameof(Audiotrack))]
    [Column("audiotrack_id")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    public PlaylistDbModel? Playlist { get; set; }
    public AudiotrackDbModel? Audiotrack { get; set; }
}