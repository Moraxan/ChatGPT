namespace ChatGPTAPI.Models
{
    public class GPTResponseModel
    {
        public List<Choice> Choices { get; set; }
        // Include other fields as per the API response
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
        // Include other fields as per the API response
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
