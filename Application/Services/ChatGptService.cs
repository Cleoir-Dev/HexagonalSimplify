using Domain.Adapters;
using Domain.Entities.ChatGpt;
using Domain.Services;

namespace Application.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly IChatGptAdapter _chatGptAdapter;

        public ChatGptService(IChatGptAdapter chatGptAdapter) =>
            _chatGptAdapter = chatGptAdapter;

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
    }
}
