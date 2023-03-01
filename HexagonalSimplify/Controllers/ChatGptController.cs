using Domain.Entities.ChatGpt;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/chat-gpt")]
    [ApiController]
    public class ChatGptController : ControllerBase
    {
        private readonly IChatGptService _chatGptService;

        public ChatGptController(IChatGptService chatGptService) =>
            _chatGptService = chatGptService;


        [HttpPost("questions-to-answers")]
        public async Task<IActionResult> Chat(string question)
        {

            var chat = await _chatGptService.Communication(new RequestGpt { inputTxt = question });

            return Ok(chat.choices[0].text);
        }

        [HttpPost("answer-to-speech")]
        public async Task<IActionResult> ChatSpeech(string question)
        {

            var result = await _chatGptService.ChatSpeech(new RequestGpt { inputTxt = question });
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
