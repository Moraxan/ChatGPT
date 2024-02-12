
namespace ChatGPTAPI.Interfaces
{
    public interface IChatGPTService
    {
        Task<string> PostQueryToChatGPTAsync(GPTRequestModel request);
    }
}