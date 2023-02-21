using Domain.Adapters;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Infra.ChatGPT.Adapters
{
    public class ChatGptAdapter : IChatGptAdapter
    {

        private readonly HttpClient _httpClient;

        public ChatGptAdapter(HttpClient httpClient, IConfiguration conf)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", conf.GetSection("OpenaiKey").Value);

        }

        public async Task<ResponseGpt> Connection(ChatGpt chatGpt)
        {
            var result = new ResponseGpt();

            var Message = await _httpClient.PostAsync("https://api.openai.com/v1/completions",
                new StringContent(JsonSerializer.Serialize(chatGpt),
            Encoding.UTF8, "application/json"));

            if (Message.IsSuccessStatusCode)
            {
                var response = await Message.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ResponseGpt>(response);
                return result;
            }

            return result;
        }
    }
    
}
