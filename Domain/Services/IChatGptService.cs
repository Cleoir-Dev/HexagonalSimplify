using Domain.Entities;

namespace Domain.Services
{
    public interface IChatGptService
    {
        Task<ResponseGpt> Communication(ChatGpt chatGpt);
    }
}
