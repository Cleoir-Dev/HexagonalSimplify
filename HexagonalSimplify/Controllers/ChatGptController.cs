using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGptController : ControllerBase
    {
        private readonly IChatGptService _chatGptService;

        public ChatGptController(IChatGptService chatGptService) =>
            _chatGptService = chatGptService;


        [HttpPost]
        public async Task<IActionResult> Chat(string prompt)
        {

            var chat = await _chatGptService.Communication(new ChatGpt { prompt = prompt }) ;

            return Ok(chat.choices[0].text);
        }
    }

}
