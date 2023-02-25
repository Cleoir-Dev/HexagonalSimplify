using Domain.Entities.Speech;

namespace Domain.Adapters
{
    public interface ISpeechAdapter
    {
        Task<ResponseSpeech> RequestAsync(RequestSpeech requestSpeech);
    }
}
