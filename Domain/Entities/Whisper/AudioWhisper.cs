using System.Text.Json.Serialization;

namespace Domain.Entities.Whisper
{
    public class AudioWhisper
    {
        [JsonPropertyName("audio")]
        public string whisper { get; set; }
        public string filename { get; set; }
        public byte[] content { get; set; }
    }
}
