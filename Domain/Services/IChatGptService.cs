using Domain.Entities.ChatGpt;
using Domain.Entities.Speech;

namespace Domain.Services
{
    public interface IChatGptService
    {
        Task<ResponseGpt> Communication(RequestGpt chatGpt);
        Task<ResponseSpeech> ChatSpeech(RequestGpt chatGpt);
    }
}
