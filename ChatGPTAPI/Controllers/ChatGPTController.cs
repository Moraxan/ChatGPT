

namespace ChatGPTAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatGPTController : ControllerBase
    {
        private readonly IChatGPTService _chatGPTService;  // Field

        public ChatGPTController(IChatGPTService chatGPTService) // Traditional Constructor
        {
            _chatGPTService = chatGPTService;
        }

        [HttpPost("query")]
        public async Task<IActionResult> PostQueryToChatGPT([FromBody] GPTRequestModel request)
        {
            try
            {
                var response = await _chatGPTService.PostQueryToChatGPTAsync(request);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
