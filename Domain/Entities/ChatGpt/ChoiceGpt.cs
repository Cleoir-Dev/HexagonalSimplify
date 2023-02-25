using System.Text.Json.Serialization;

namespace Domain.Entities.ChatGpt
{
    public class ChoiceGpt
    {
        public string text { get; set; }
        public int index { get; set; }

        [JsonPropertyName("logprobs")]
        public object log { get; set; }
        [JsonPropertyName("finish_reason")]
        public string release { get; set; }
    }
}
