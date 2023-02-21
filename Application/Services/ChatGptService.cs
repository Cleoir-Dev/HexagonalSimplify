using Domain.Adapters;
using Domain.Entities;
using Domain.Services;

namespace Application.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly IChatGptAdapter _chatGptAdapter;

        public ChatGptService(IChatGptAdapter chatGptAdapter) =>
            _chatGptAdapter = chatGptAdapter;

        public async Task<ResponseGpt> Communication(ChatGpt chatGpt)
        {
            // Detalhamento de resposta de saida e tambem a concordancia verbal.
            chatGpt.model = "text-davinci-003";
            //A temperatura é o acerto na resposta vai de 0 a 2 quanto menor mais preciso.
            chatGpt.temperature = 0;
            //Quantidade de caracteres retornados no text. Exemplo 4 caracteres para um token.
            chatGpt.max_tokens = 2048;


            var chat = await _chatGptAdapter.Connection(chatGpt);

            return chat;
        }
    }
}
