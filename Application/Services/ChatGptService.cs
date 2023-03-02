using Domain.Adapters;
using Domain.Entities.ChatGpt;
using Domain.Entities.Speech;
using Domain.Services;

namespace Application.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly IChatGptAdapter _chatGptAdapter;
        private readonly ISpeechService _speechService;

        public ChatGptService(IChatGptAdapter chatGptAdapter, ISpeechService speechService)
        {
            _chatGptAdapter = chatGptAdapter;
            _speechService = speechService;
        }


        public async Task<ResponseGpt> Communication(RequestGpt chatGpt)
        {
            // Output response detailing and also verbal agreement.
            chatGpt.formatTxt = "text-davinci-003";
            // Precision is the correct answer ranging from 0 to 2, the smaller, the more accurate.
            chatGpt.precision = 0;
            // Number of characters returned in the text. Example 4 characters represents 1 for chatGpt.
            // The accuracy is the correct response ranges from 0 to 2, the smaller, the more accurate.
            chatGpt.qtdCaract = 2048;


            var chat = await _chatGptAdapter.RequestAsync(chatGpt);

            return chat;
        }

        public async Task<ResponseSpeech> ChatSpeech(RequestGpt chatGpt)
        {
            var chat = await Communication(chatGpt);

            var requestSpeech = new RequestSpeech
            {
                text = chat.choices[0].text
            };

            var response = await _speechService.TextToSpeech(requestSpeech);

            return response;
        }
    }
}
