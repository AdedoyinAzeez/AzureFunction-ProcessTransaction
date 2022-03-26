using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Pyrros.Interface;
using Pyrros.Models;

namespace Pyrros
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly ITransactionService transaction;

        public Function1(ILogger<Function1> log, ITransactionService transaction)
        {
            _logger = log;
            this.transaction = transaction;
        }

        [FunctionName("ProcessTransaction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "ProcessTransaction" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Description = "Transaction Object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Success", Summary = "This endpoint executes transaction anad inserts it into Transaction table")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(object), Description = "Bad Request", Summary = "This endpoint executes transaction anad inserts it into Transaction table")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(object), Description = "Internal Server Error", Summary = "This endpoint executes transaction anad inserts it into Transaction table")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
            
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<Transaction>(requestBody);

                ServiceResponse post = await transaction.InsertTransaction(data);

                return !post.Success ? new BadRequestObjectResult(post.Message) : new OkObjectResult(post.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "");
                return new NotFoundResult();
            }
        }
    }
}

