using Newtonsoft.Json;

namespace MewingPad.UI.DTOs;

[JsonObject]
public class CommentaryDto(Guid id,
                           Guid authorId,
                           Guid audiotrackId,
                           string text)
{
    [JsonProperty("id")]
    public Guid Id { get; set; } = id;

    [JsonProperty("audiotrackId")]
    public Guid AudiotrackId { get; set; } = audiotrackId;

    [JsonProperty("authorId")]
    public Guid AuthorId { get; set; } = authorId;

    [JsonProperty("text")]
    public string Text { get; set; } = text;
}
