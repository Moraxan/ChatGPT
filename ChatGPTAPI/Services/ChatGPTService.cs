namespace ChatGPTAPI.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiKey;
        private const string OpenAiEndpoint = "https://api.openai.com/v1/chat/completions";

        public ChatGPTService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiKey = configuration["GPT_API_KEY"];
        }

        public async Task<string> PostQueryToChatGPTAsync(GPTRequestModel request)
        {
            var client = _clientFactory.CreateClient();

            var openAiRequest = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = request.Prompt } },
                temperature = request.Temperature,
                max_tokens = request.MaxTokens
            };

            string jsonContent = JsonSerializer.Serialize(openAiRequest);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await client.PostAsync(OpenAiEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var chatGPTResponse = JsonSerializer.Deserialize<GPTResponseModel>(responseContent, options);

                if (chatGPTResponse?.Choices == null || !chatGPTResponse.Choices.Any())
                {
                    throw new InvalidOperationException("No choices available in the response.");
                }

                var firstResponseContent = chatGPTResponse.Choices.FirstOrDefault()?.Message?.Content;
                if (string.IsNullOrEmpty(firstResponseContent))
                {
                    throw new InvalidOperationException("Received null or empty content in the response.");
                }

                // Serialize for JSON consistency on success 
                return JsonSerializer.Serialize(chatGPTResponse);
            }
            else
            {
                // Consistent JSON responses on error too
                var errorResponse = new { error = $"API error: {responseContent}" };
                var serializedError = JsonSerializer.Serialize(errorResponse);
                return serializedError;
            }
        }
    }
}
