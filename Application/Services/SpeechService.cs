using Domain.Adapters;
using Domain.Entities.Speech;
using Domain.Services;

namespace Application.Services
{
    public class SpeechService : ISpeechService
    {
        private readonly ISpeechAdapter _speechAdapter;

        public SpeechService(ISpeechAdapter speechAdapter)
        {
            _speechAdapter = speechAdapter;
        }

        public async Task<ResponseSpeech> TextToSpeech(RequestSpeech requestSpeech)
        {
            // Get the voice timbre id
            requestSpeech.voiceId = "21m00Tcm4TlvDq8ikWAM";
            // Parameters equalize the sound
            requestSpeech.voiceSettings = new VoiceSettings
            {
                similarityBoost = 0.6,
                stability = 0.6,
            };

            var speech = await _speechAdapter.RequestAsync(requestSpeech);
            return speech;

        }
    }
}
