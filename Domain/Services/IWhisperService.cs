using Domain.Entities.Whisper;

namespace Domain.Services
{
    public interface IWhisperService
    {
        Task<ResponseWhisper> AudioToText(RequestWhisper requestWhisper);
        Task<ResponseWhisper> GetStatusAndResponse(string uuid);
    }
}
