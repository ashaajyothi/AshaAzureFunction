using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
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

        [Function("TimerTriggerCSharp")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        [Function("CosmosTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "databaseName",
            containerName: "containerName",
            Connection = "CosmosDBConnectionSetting",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<ToDoItem> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].id);
            }
        }

        [Function("BlobTrigger")]
        public static void Run([BlobTrigger("samples-workitems/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }

    public class ToDoItem
    {
        public string id { get; set; }
        public string Description { get; set; }
    }
}
