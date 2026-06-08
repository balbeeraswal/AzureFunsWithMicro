using System.Net;
using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FuncAppWithMicroServices;

public class HttpTriggerFunc
{
    private readonly ILogger<HttpTriggerFunc> _logger;

    public HttpTriggerFunc(ILogger<HttpTriggerFunc> logger)
    {
        _logger = logger;
    }

    [Function("HttpTriggerFunc")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",
        Route = "Employee/{id}")] HttpRequestData req, int id,FunctionContext executionContext)
    {

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var identity = req.Identities.FirstOrDefault();
        
      
        if (identity == null || !identity.IsAuthenticated)
        {
             var unauthoriesedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
            return unauthoriesedResponse;
        }

        var user = new ClaimsPrincipal(identity);

        if(!user.IsInRole("Admin"))
        {
            _logger.LogInformation("Non-admin user accessed the function.");
            var forbidResponse = req.CreateResponse(HttpStatusCode.Forbidden);
            return forbidResponse;
          
            
        }
        var employeeData = new { Id = id, Name = "John Doe", Role = "Developer" };

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(employeeData);
        return response;
    }
}