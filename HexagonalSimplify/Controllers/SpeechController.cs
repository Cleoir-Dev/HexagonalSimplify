using Domain.Entities.Speech;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/speech")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly ISpeechService _speechService;

        public SpeechController(ISpeechService speechService)
        {
            _speechService = speechService;
        }

        [HttpPost("text-to-speech")]
        public async Task<IActionResult> TextToSpeech(string text)
        {
            var result = await _speechService.TextToSpeech(new RequestSpeech { text = text });
            if (result.IsSuccess)
            {
                var memoryStream = new MemoryStream(result.Content);
                memoryStream.Position = 0;
                return File(memoryStream, "audio/mpeg", result.FileName);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
