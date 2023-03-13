using Domain.Adapters;
using Domain.Entities.Speech;
using Domain.Entities.Whisper;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infra.Adapters
{
    public class WhisperAdapter : IWhisperAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly String REPLICATE_URL_API = "https://api.replicate.com/v1/predictions";

        public WhisperAdapter(HttpClient httpClient, IConfiguration conf)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Token", conf.GetSection("ReplicateKey").Value);

        }

        public async Task<ResponseWhisper> PostAsync(RequestWhisper requestWhisper)
        {

            var response = await _httpClient.PostAsync($"{REPLICATE_URL_API}",
                new StringContent(JsonSerializer.Serialize(requestWhisper),
            Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var contentDisposition = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseWhisper>(contentDisposition);
                result.IsSuccess = true;
                return result;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new ResponseWhisper
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error in the convert audio to text: {errorMessage}"
                };
            }
        }

        public async Task<ResponseWhisper> GetAsync(string uuid)
        {

            var response = await _httpClient.GetAsync($"{REPLICATE_URL_API}/{uuid}");

            if (response.IsSuccessStatusCode)
            {
                var contentDisposition = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseWhisper>(contentDisposition);
                result.IsSuccess = true;
                return result;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new ResponseWhisper
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error in capture status, text: {errorMessage}"
                };
            }
        }
    }
}
