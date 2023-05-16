using Domain.Adapters;
using Domain.Entities.Speech;
using Domain.Services;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NAudio.Lame;
using NAudio.Wave;
using System.IO.Compression;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using Path = System.IO.Path;

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

        public async Task<string> ConvertPDFToAudiobook(Stream stream)
        {
            // Lê o texto do PDF
            string text = await ReadPdfText(stream);

            // Converte o texto em um arquivo de áudio no formato WAV
            var wavAudioFile = await ConvertTextToSpeech(text);

            // Converte o arquivo de áudio WAV para MP3
            var mp3AudioFile = await ConvertWavToMp3(wavAudioFile);

            // Cria o HLS
            var hlsFiles = await CreateHlsFiles(mp3AudioFile);

            // Retorna o HLS como resultado da requisição
            return hlsFiles;
        }

        public async Task<string> ReadPdfText(Stream stream)
        {

            using (var reader = new PdfReader(stream))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }

        }

        public async Task<byte[]> ConvertTextToSpeech(string text)
        {
            var tempAudioFile = Path.GetTempFileName();
            using (var synthesizer = new SpeechSynthesizer())
            {
                var audioFormat = new SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);

                synthesizer.SetOutputToWaveFile(tempAudioFile, audioFormat);
                synthesizer.Speak(text);
            }

            var audioBytes = await File.ReadAllBytesAsync(tempAudioFile);
            File.Delete(tempAudioFile);

            return audioBytes;
        }

        public async Task<string> ConvertWavToMp3(byte[] wavAudio)
        {
            using (var wavStream = new MemoryStream(wavAudio))
            {
                using (var mp3Stream = new MemoryStream())
                {
                    using (var reader = new WaveFileReader(wavStream))
                    {
                        using (var writer = new LameMP3FileWriter(mp3Stream, reader.WaveFormat, LAMEPreset.STANDARD))
                        {
                            reader.CopyTo(writer);
                        }
                    }

                    return Convert.ToBase64String(mp3Stream.ToArray());
                }
            }
        }

        public async Task<string> CreateHlsFiles(string mp3Audio)
        {
            // Tamanho do buffer em bytes
            const int BufferSize = 4096; 
            // Diretório temporário para armazenar os arquivos HLS
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            // Caminho completo para o arquivo MP3 temporário
            var mp3FilePath = Path.Combine(tempDirectory, "audio.mp3");
            File.WriteAllBytes(mp3FilePath, Convert.FromBase64String(mp3Audio));

            // Caminho completo para o arquivo de índice (playlist)
            var indexFilePath = Path.Combine(tempDirectory, "index.m3u8");

            // Configuração do HLS
            var playlistPath = "audiobook"; // Caminho relativo para os segmentos do áudio
            var segmentDuration = 10; // Duração de cada segmento em segundos
            var playlistDuration = 0; // Duração total da playlist

            using (var writer = new StreamWriter(indexFilePath))
            {
                writer.WriteLine("#EXTM3U");
                writer.WriteLine("#EXT-X-VERSION:3");
                writer.WriteLine("#EXT-X-TARGETDURATION:" + segmentDuration);
                writer.WriteLine("#EXT-X-MEDIA-SEQUENCE:0");

                var segmentIndex = 0;
                var segmentFilePath = Path.Combine(tempDirectory, $"{segmentIndex}.ts");

                using (var mp3Reader = new Mp3FileReader(mp3FilePath))
                {
                    var sampleRate = mp3Reader.Mp3WaveFormat.SampleRate;
                    var bytesPerSegment = sampleRate * segmentDuration * mp3Reader.Mp3WaveFormat.Channels * 2;

                    while (mp3Reader.Position < mp3Reader.Length)
                    {
                        using (var segmentWriter = new FileStream(segmentFilePath, FileMode.Create))
                        {
                            var buffer = new byte[BufferSize];
                            var bytesRead = 0;

                            while (bytesRead < bytesPerSegment && (bytesRead = mp3Reader.Read(buffer, 0, BufferSize)) > 0)
                            {
                                segmentWriter.Write(buffer, 0, bytesRead);
                                bytesRead += bytesRead;
                            }

                            segmentWriter.Flush();
                            segmentWriter.Close();
                        }

                        writer.WriteLine($"#EXTINF:{segmentDuration},");
                        writer.WriteLine(Path.Combine(playlistPath, $"{segmentIndex}.ts"));

                        playlistDuration += segmentDuration;
                        segmentIndex++;

                        segmentFilePath = Path.Combine(tempDirectory, $"{segmentIndex}.ts");
                    }
                }

                writer.WriteLine("#EXT-X-ENDLIST");
                writer.Flush();
                writer.Close();
            }

            // Compactar arquivos HLS em um arquivo ZIP
            var zipFilePath = Path.Combine(Path.GetTempPath(), $"audiobook-{Guid.NewGuid()}.zip");
            ZipFile.CreateFromDirectory(tempDirectory, zipFilePath);

            // Excluir arquivos temporários e diretório
            File.Delete(mp3FilePath);
            Directory.Delete(tempDirectory, true);

            return zipFilePath;
        }
    }
}
