using System.Text.Json.Serialization;

namespace Domain.Entities.ChatGpt
{
    public class ResponseGpt
    {
        public string id { get; set; }

        [JsonPropertyName("object")]
        public string result { get; set; }

        public int created { get; set; }

        [JsonPropertyName("model")]
        public string formatTxt { get; set; }

        public List<ChoiceGpt> choices { get; set; }

        public UsageGpt usage { get; set; }
    }
}
