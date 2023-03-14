using System.Text.Json.Serialization;

namespace Domain.Entities.Whisper
{
    public class ResponseWhisper
    {
        public string id { get; set; }

        [JsonPropertyName("input")]
        public AudioWhisper audio { get; set; }

        public string status { get; set; }

        public string version { get; set; }

        [JsonPropertyName("completed_at")]
        public object completed_at { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime created_at { get; set; }

        public object error { get; set; }

        public object logs { get; set; }

        public object output { get; set; }

        [JsonPropertyName("started_at")]
        public object started_at { get; set; }
        public bool IsSuccess { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
