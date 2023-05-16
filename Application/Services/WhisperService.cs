using Domain.Adapters;
using Domain.Entities.ChatGpt;
using Domain.Entities.Speech;
using Domain.Entities.Whisper;
using Domain.Services;

namespace Application.Services
{
    public class WhisperService : IWhisperService
    {
        private readonly IWhisperAdapter _whisperAdapter;
        private readonly IChatGptService _chatGptService;

        public WhisperService(IWhisperAdapter whisperAdapter, IChatGptService chatGptService)
        {
            _whisperAdapter = whisperAdapter;
            _chatGptService = chatGptService;
        }

        public async Task<ResponseWhisper> AudioToText(RequestWhisper requestWhisper)
        {
            //Defined prediction. Get prediction on the site replicate. Best I.A version.
            requestWhisper.version = "e39e354773466b955265e969568deb7da217804d8e771ea8c9cd0cef6591f8bc";

            // format for the transcription
            requestWhisper.audio.translate = true;
            requestWhisper.audio.model = "large";
            requestWhisper.audio.transcription = "plain text";
            requestWhisper.audio.suppress_tokens = "-1";
            requestWhisper.audio.logprob_threshold = -1;
            requestWhisper.audio.no_speech_threshold = 0.6;
            requestWhisper.audio.condition_on_previous_text = true;
            requestWhisper.audio.compression_ratio_threshold = 2.4;
            requestWhisper.audio.temperature_increment_on_fallback = 0.2;


            var whisper = await _whisperAdapter.PostAsync(requestWhisper);
            return whisper;
        }

        public async Task<ResponseWhisper> GetStatusAndResponse(string uuid)
        {
            var whisper = await _whisperAdapter.GetAsync(uuid);
            return whisper;
        }

        public async Task<ResponseSpeech> AudioPortugueseBR(RequestWhisper requestWhisper)
        {
            ResponseSpeech result = new ResponseSpeech();

            var whisper = await AudioToText(requestWhisper);

            var conversion = new ResponseWhisper();
            conversion.status = "processing";

            do
            {
                conversion = await GetStatusAndResponse(whisper.id);

                if (conversion.status == "succeeded")
                {
                    result = await _chatGptService.ChatSpeech(new RequestGpt
                    {
                        inputTxt = $"Translate the following text into Portuguese BR and rewrite it in more common language: {conversion.output.transcription}"
                    });
                }

            } while (conversion.status == "processing" || conversion.status == "starting");

            return result;
        }
    }
}
