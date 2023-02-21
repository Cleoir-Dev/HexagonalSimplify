using Domain.Entities;

namespace Domain.Adapters
{
    public interface IChatGptAdapter
    {
        Task<ResponseGpt> Connection(ChatGpt chatGpt);
    }
}
