using Domain.Adapters;
using Domain.Entities.Speech;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Infra.ChatGPT.Adapters
{
    public class SpeechAdapter : ISpeechAdapter
    {

        private readonly HttpClient _httpClient;

        public SpeechAdapter(HttpClient httpClient, IConfiguration conf)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("xi-api-key", conf.GetSection("ElevenLabsKey").Value);

        }

        public async Task<ResponseSpeech> RequestAsync(RequestSpeech requestSpeech)
        {
            var response = await _httpClient.PostAsync($"https://api.elevenlabs.io/v1/text-to-speech/{requestSpeech.voiceId}",
                new StringContent(JsonSerializer.Serialize(requestSpeech),
            Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var contentDisposition = response.Content.Headers.ContentDisposition;
                var fileName = contentDisposition?.FileName ?? "audio.mp3";

                var memoryStream = new MemoryStream();
                await response.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return new ResponseSpeech
                {
                    IsSuccess = true,
                    Content = memoryStream.ToArray(),
                    FileName = fileName
                };

            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new ResponseSpeech
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error in the convert text to audio: {errorMessage}"
                };
            }
        }
    }
}
