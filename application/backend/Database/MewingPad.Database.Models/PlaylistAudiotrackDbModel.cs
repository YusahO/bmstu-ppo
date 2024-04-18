using System.ComponentModel.DataAnnotations.Schema;

namespace MewingPad.Database.Models;

[Table("PlaylistsAudiotracks")]
public class PlaylistAudiotrackDbModel(Guid playlistId,
                                       Guid audiotrackId)
{
    public Guid PlaylistId { get; set; } = playlistId;
    public Guid AudiotrackId { get; set; } = audiotrackId;

    public PlaylistDbModel Playlist { get; set; } = null!;
    public AudiotrackDbModel Audiotrack { get; set; } = null!;
}