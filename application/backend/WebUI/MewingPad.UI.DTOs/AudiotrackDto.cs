using Newtonsoft.Json;

namespace MewingPad.UI.DTOs;

[JsonObject]
public class AudiotrackDto(Guid id,
                          string title,
                          float duration,
                          Guid authorId,
                          string filepath)
{
    [JsonProperty("audiotrackId")]
    public Guid Id { get; set; } = id;

    [JsonProperty("title")]
    public string Title { get; set; } = title;

    [JsonProperty("duration")]
    public float Duration { get; set; } = duration;

    [JsonProperty("authorId")]
    public Guid AuthorId { get; set; } = authorId;

    [JsonProperty("filepath")]
    public string Filepath { get; set; } = filepath;
}
