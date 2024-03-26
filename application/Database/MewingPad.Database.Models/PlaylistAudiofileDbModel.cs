namespace MewingPad.Database.Models;

public class PlaylistAudiofileDbModel(Guid playlistId,
                                      Guid audiofileId)
{
    public Guid PlaylistId { get; set; } = playlistId;
    public Guid AudiofileId { get; set; } = audiofileId;

    public PlaylistDbModel Playlist { get; set; } = null!;
    public AudiofileDbModel Audiofile { get; set; } = null!;
}