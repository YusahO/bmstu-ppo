using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MewingPad.UI.DTOs;

[JsonObject]
public class AudiotrackFormDto
{
    [JsonProperty("audiotrackId")]
	public Guid Id { get; set; }

	[JsonProperty("title")]
	public string Title { get; set; }

	[JsonProperty("duration")]
	public float Duration { get; set; }

	[JsonProperty("authorId")]
	public Guid AuthorId { get; set; }

	[JsonProperty("filepath")]
	public string Filepath { get; set; }

	public IFormFile File { get; set; }

	public AudiotrackFormDto() {}

    public AudiotrackFormDto(Guid id,
							 string title,
							 float duration,
							 Guid authorId,
							 string filepath)
    {
        Id = id;
        Title = title;
        Duration = duration;
        AuthorId = authorId;
        Filepath = filepath;
    }
}
