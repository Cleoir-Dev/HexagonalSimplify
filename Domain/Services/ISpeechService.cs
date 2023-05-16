using Domain.Entities.Speech;

namespace Domain.Services
{
    public interface ISpeechService
    {
        Task<ResponseSpeech> TextToSpeech(RequestSpeech requestSpeech);
        Task<string> ReadPdfText(Stream stream);
        Task<byte[]> ConvertTextToSpeech(string text);
        Task<string> ConvertWavToMp3(byte[] wavAudio);
        Task<string> CreateHlsFiles(string mp3Audio);
        Task<string> ConvertPDFToAudiobook(Stream stream);
    }
}
