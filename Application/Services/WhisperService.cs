using Domain.Adapters;
using Domain.Entities.Whisper;
using Domain.Services;

namespace Application.Services
{
    public class WhisperService : IWhisperService
    {
        private readonly IWhisperAdapter _whisperAdapter;

        public WhisperService(IWhisperAdapter whisperAdapter)
        {
            _whisperAdapter = whisperAdapter;
        }

        public async Task<ResponseWhisper> AudioToText(RequestWhisper requestWhisper)
        {
            //Defined prediction
            requestWhisper.version = "e39e354773466b955265e969568deb7da217804d8e771ea8c9cd0cef6591f8bc";
            var whisper = await _whisperAdapter.PostAsync(requestWhisper);
            return whisper;
        }

        public async Task<ResponseWhisper> GetStatusAndResponse(string uuid)
        {
            var whisper = await _whisperAdapter.GetAsync(uuid);
            return whisper;
        }
    }
}
