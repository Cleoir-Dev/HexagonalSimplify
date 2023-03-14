using Domain.Entities.Speech;
using Domain.Entities.Whisper;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Controllers
{
    [Route("api/whisper")]
    [ApiController]
    public class WhisperController : ControllerBase
    {
        private readonly IWhisperService _whisperService;

        public WhisperController(IWhisperService whisperService)
        {
            _whisperService = whisperService;
        }

        [HttpPost("whisper-to-text")]
        public async Task<IActionResult> WhisperToText(string urlAudio)
        {
            var result = await _whisperService.AudioToText(new RequestWhisper
            {
                audio = new AudioWhisper { whisper = urlAudio },
            });

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpGet("whisper-status-text")]
        public async Task<IActionResult> WhisperStatusText(string uuid)
        {
            var result = await _whisperService.GetStatusAndResponse(uuid);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpPost("translate-audio-to-portugues")]
        public async Task<IActionResult> TranslateAudioToPortugues(string urlAudio)
        {
            var result = await _whisperService.AudioPortugueseBR(new RequestWhisper
            {
                audio = new AudioWhisper { whisper = urlAudio },
            });

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
