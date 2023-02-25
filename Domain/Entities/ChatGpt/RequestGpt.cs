using System.Text.Json.Serialization;

namespace Domain.Entities.ChatGpt
{
    public class RequestGpt
    {
        [JsonPropertyName("model")]
        public string formatTxt { get; set; }

        [JsonPropertyName("prompt")]
        public string inputTxt { get; set; }

        [JsonPropertyName("temperature")]
        public decimal precision { get; set; }

        [JsonPropertyName("max_tokens")]
        public decimal qtdCaract { get; set; }
    }


}
