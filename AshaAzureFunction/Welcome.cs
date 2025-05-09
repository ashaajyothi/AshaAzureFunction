using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AshaAzureFunction
{
    public class Welcome
    {
        private readonly ILogger<Welcome> _logger;

        public Welcome(ILogger<Welcome> logger)
        {
            _logger = logger;
        }

        [Function("Welcome")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function started processing a request.");

            try
            {
                // Example: Validate input (if applicable)
                if (req.Method == "POST" && req.ContentLength == 0)
                {
                    _logger.LogWarning("Empty POST request received.");
                    return new BadRequestObjectResult("Request body cannot be empty.");
                }

                // Simulate async operation (if needed)
                await Task.Delay(10);

                _logger.LogInformation("C# HTTP trigger function successfully processed the request.");
                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
