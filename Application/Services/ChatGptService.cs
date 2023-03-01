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
            // Detalhamento de resposta de saida e tambem a concordancia verbal.
            chatGpt.formatTxt = "text-davinci-003";
            //A precisao é o acerto na resposta vai de 0 a 2 quanto menor mais preciso.
            chatGpt.precision = 0;
            //Quantidade de caracteres retornados no text. Exemplo 4 caracteres representa 1 para chatGpt.
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
