using System.Text.Json.Serialization;

namespace HackerNews.Client;

public record HackerNewsItem
{
    /// <summary>
    /// The item's unique id.
    /// </summary>
    [JsonPropertyName("id")]
    public required long Id { get; init; }

    /// <summary>
    /// True if the item is deleted.
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool? Deleted { get; init; }

    /// <summary>
    /// The type of item. One of "job", "story", "comment", "poll", or "pollopt".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// The username of the item's author.
    /// </summary>
    [JsonPropertyName("by")]
    public string? By { get; init; }

    /// <summary>
    /// Creation date of the item, in Unix Time.
    /// </summary>
    [JsonPropertyName("time")]
    public long? Time { get; init; }

    /// <summary>
    /// The comment, story or poll text. HTML.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// True if the item is dead.
    /// </summary>
    [JsonPropertyName("dead")]
    public bool? Dead { get; init; }

    /// <summary>
    /// The comment's parent: either another comment or the relevant story.
    /// </summary>
    [JsonPropertyName("parent")]
    public int? Parent { get; init; }

    /// <summary>
    /// The pollopt's associated poll.
    /// </summary>
    [JsonPropertyName("poll")]
    public int? Poll { get; init; }

    /// <summary>
    /// The ids of the item's comments, in ranked display order.
    /// </summary>
    [JsonPropertyName("kids")]
    public IEnumerable<int>? Kids { get; init; }

    /// <summary>
    /// The URL of the story.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// The story's score, or the votes for a pollopt.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; init; } = 0;

    /// <summary>
    /// The title of the story, poll or job. HTML.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// A list of related pollopts, in display order.
    /// </summary>
    [JsonPropertyName("parts")]
    public IEnumerable<int>? Parts { get; init; }

    /// <summary>
    /// In the case of stories or polls, the total comment count.
    /// </summary>
    [JsonPropertyName("descendants")]
    public int? Descendants { get; init; }
}
