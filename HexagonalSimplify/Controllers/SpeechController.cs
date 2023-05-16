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

        [HttpPost("convert-pdf-audiobook")]
        public async Task<IActionResult> ConvertPDFToAudiobook(IFormFile pdfFile)
        {
            try
            {
                // Verifica se foi enviado um arquivo PDF na requisição
                if (pdfFile == null || pdfFile.Length == 0)
                    return BadRequest("Nenhum arquivo PDF foi enviado.");

                var stream = pdfFile.OpenReadStream();
                var hlsFiles = await _speechService.ConvertPDFToAudiobook(stream);

                return Ok(hlsFiles);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao converter o PDF para audiobook: " + e);
            }
        }

    }
}
