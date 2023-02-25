using System.Text.Json.Serialization;

namespace Domain.Entities.Speech
{
    public class RequestSpeech
    {
        [JsonIgnore]
        public string voiceId { get; set; }
        public string text { get; set; }

        [JsonPropertyName("voice_settings")]
        public VoiceSettings voiceSettings { get; set; }
    }
}
