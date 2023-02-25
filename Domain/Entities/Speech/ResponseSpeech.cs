namespace Domain.Entities.Speech
{
    public class ResponseSpeech
    {
        public bool IsSuccess { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
