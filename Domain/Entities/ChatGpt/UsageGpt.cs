using System.Text.Json.Serialization;

namespace Domain.Entities.ChatGpt
{
    public class UsageGpt
    {
        [JsonPropertyName("prompt_tokens")]
        public int qtdPrompt { get; set; }
        [JsonPropertyName("completion_tokens")]
        public int qtdCompletion { get; set; }
        [JsonPropertyName("total_tokens")]
        public int total { get; set; }
    }
}
