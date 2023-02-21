namespace Domain.Entities
{
    public class ChatGpt
    {
        public string model { get; set; }
        public string prompt { get; set; }
        public decimal temperature { get; set; }
        public decimal max_tokens { get; set; }
    }

    public class ResponseGpt
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public List<ChoiceGpt> choices { get; set; }
        public UsageGpt usage { get; set; }
    }

    public class ChoiceGpt
    {
        public string text { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    public class UsageGpt
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
