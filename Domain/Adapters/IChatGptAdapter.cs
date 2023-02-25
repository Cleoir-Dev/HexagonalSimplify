using Domain.Entities.ChatGpt;

namespace Domain.Adapters
{
    public interface IChatGptAdapter
    {
        Task<ResponseGpt> RequestAsync(RequestGpt chatGpt);
    }
}
