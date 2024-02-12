namespace ChatGPTAPI.Models
{
    public class GPTRequestModel
    {
        public string Prompt { get; set; }
        public double? Temperature { get; set; }
        public int? MaxTokens { get; set; }
        // Other parameters as needed
    }

}
