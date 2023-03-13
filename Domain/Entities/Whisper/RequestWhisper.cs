using System.Text.Json.Serialization;

namespace Domain.Entities.Whisper
{
    public class RequestWhisper
    {
        public string id { get; set; }
        [JsonPropertyName("input")]
        public AudioWhisper audio { get; set; }
        public string status { get; set; }
        public string version { get; set; }
    }
}
