using System.Text.Json.Serialization;

namespace Domain.Entities.Whisper
{
    public class AudioWhisper
    {
        [JsonPropertyName("audio")]
        public string whisper { get; set; }
        public string model { get; set; }
        public bool translate { get; set; }
        public string transcription { get; set; }
        public string suppress_tokens { get; set; }
        public int logprob_threshold { get; set; }
        public double no_speech_threshold { get; set; }
        public bool condition_on_previous_text { get; set; }
        public double compression_ratio_threshold { get; set; }
        public double temperature_increment_on_fallback { get; set; }
    }
}
