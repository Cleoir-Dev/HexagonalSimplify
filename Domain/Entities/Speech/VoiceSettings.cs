using System.Text.Json.Serialization;

namespace Domain.Entities.Speech
{
    public class VoiceSettings
    {
        public double stability { get; set; }

        [JsonPropertyName("similarity_boost")]
        public double similarityBoost { get; set; }
    }
}
