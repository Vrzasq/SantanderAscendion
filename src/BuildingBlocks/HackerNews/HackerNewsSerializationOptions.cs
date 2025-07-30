using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerNews;

internal class HackerNewsSerializationOptions
{
    public const string Name = nameof(HackerNewsSerializationOptions);

    public static JsonSerializerOptions Default => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReadCommentHandling = JsonCommentHandling.Skip,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
}
