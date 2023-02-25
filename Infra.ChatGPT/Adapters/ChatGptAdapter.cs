using Domain.Adapters;
using Domain.Entities.ChatGpt;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

        public async Task<ResponseGpt> RequestAsync(RequestGpt chatGpt)
        {
            var result = new ResponseGpt();

            var message = await _httpClient.PostAsync("https://api.openai.com/v1/completions",
                new StringContent(JsonSerializer.Serialize(chatGpt),
            Encoding.UTF8, "application/json"));

            if (message.IsSuccessStatusCode)
            {
                var response = await message.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ResponseGpt>(response);
                return result;
            }

            return result;
        }
    }

}
