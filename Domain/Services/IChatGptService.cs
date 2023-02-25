using Domain.Entities.ChatGpt;

namespace Domain.Services
{
    public interface IChatGptService
    {
        Task<ResponseGpt> Communication(RequestGpt chatGpt);
    }
}
