namespace Domain.Entities.Whisper
{
    public class OutputWhisper
    {
        public object translation { get; set; }
        public string transcription { get; set; }
        public string detected_language { get; set; }
    }
}
