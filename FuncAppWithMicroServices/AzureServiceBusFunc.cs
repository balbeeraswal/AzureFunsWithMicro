using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FuncAppWithMicroServices;

public class AzureServiceBusFunc
{
    private readonly ILogger<AzureServiceBusFunc> _logger;

    public AzureServiceBusFunc(ILogger<AzureServiceBusFunc> logger)
    {
        _logger = logger;
    }

    [Function(nameof(AzureServiceBusFunc))]
    public async Task Run(
        [ServiceBusTrigger("myqueue", Connection = "AzureServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}