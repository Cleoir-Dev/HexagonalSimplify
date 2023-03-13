using Domain.Entities.Whisper;

namespace Domain.Adapters
{
    public interface IWhisperAdapter
    {
        Task<ResponseWhisper> PostAsync(RequestWhisper requestWhisper);
        Task<ResponseWhisper> GetAsync(string uuid);
    }
}
