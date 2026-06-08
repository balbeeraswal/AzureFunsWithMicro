using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FuncAppWithMicroServices;

public class AzureServiceBusFunc
{
    private readonly ILogger<AzureServiceBusFunc> _logger;
    private readonly HttpClient _httpClient; 

    public AzureServiceBusFunc(ILogger<AzureServiceBusFunc> logger, HttpClient httpClient)
    {
        _logger = logger; 
        _httpClient = httpClient;
    }

    [Function(nameof(AzureServiceBusFunc))]
    public async Task Run(
        [ServiceBusTrigger("myqueue", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions, HttpClient htpClient)
    {    _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        Console.WriteLine($"Received message: {message}");

        //Call Another Api and pass received message to that Api
        var body = message.Body;
        
        // Deserialize message into Employee object
        var employee = JsonSerializer.Deserialize<Employee>(body);

         // Prepare request body
        var Jsonbody =JsonSerializer.Serialize(employee);
        var content = new StringContent(Jsonbody, Encoding.UTF8, "application/json");

        // Call another API (replace with your actual URL)
        var response = await _httpClient.PostAsync("https://localhost:7283/api/Employee/ProcessEmployee", content);

        if (response.IsSuccessStatusCode)
        {

            Console.WriteLine($"Message forwarded successfully to API.: {message}");
            _logger.LogInformation("Message forwarded successfully to API.");
        }
        else
        {
            Console.WriteLine($"Failed to forward message. Status: {response.StatusCode}");
            _logger.LogError($"Failed to forward message. Status: {response.StatusCode}");
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }

    
}

public class Employee
{
    [Key]
    [JsonIgnore]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmpId { get; set; }

    public string EmpName { get; set; }
    public int DeptId { get; set; }
    public int LocId { get; set; }

}