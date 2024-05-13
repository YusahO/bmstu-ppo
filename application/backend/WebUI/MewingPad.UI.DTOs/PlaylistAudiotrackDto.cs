using Newtonsoft.Json;

namespace MewingPad.UI.DTOs;

[JsonObject]
public class PlaylistAudiotrackDto(Guid audiotrackId, Guid playlistId)
{
	[JsonProperty("audiotrackId")]
	public Guid AudiotrackId { get; set; } = audiotrackId;

	[JsonProperty("playlistId")]
	public Guid PlaylistId { get; set; } = playlistId;
}