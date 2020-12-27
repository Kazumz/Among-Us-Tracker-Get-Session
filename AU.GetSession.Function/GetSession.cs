using System;
using System.Threading.Tasks;
using AU.GetSession.Domain.GetSession;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AU.GetSession.Function
{
    public class GetSession
    {
        private readonly IGetSessionHandler handler;

        public GetSession(IGetSessionHandler handler)
        {
            this.handler = handler;
        }

        [FunctionName("GetSession")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger logger)
        {
            try
            {
                string sessionId = req.Query["sessionId"];
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return new BadRequestObjectResult($"Session ID must be provided.");
                }

                var players = await handler.GetSession(Guid.Parse(sessionId));

                logger.LogInformation($"Processing Request Succeeded for Session ID: {sessionId}");
                return new OkObjectResult(players);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failure in Get Session Function. Unable to process");
                throw;
            }
        }
    }
}
