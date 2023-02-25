using Domain.Entities.Speech;

namespace Domain.Services
{
    public interface ISpeechService
    {
        Task<ResponseSpeech> TextToSpeech(RequestSpeech requestSpeech);
    }
}
