using Domain.Entities.ChatGpt;
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
        public async Task<IActionResult> Chat(string question)
        {

            var chat = await _chatGptService.Communication(new RequestGpt { inputTxt = question });

            return Ok(chat.choices[0].text);
        }
    }

}
