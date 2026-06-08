using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FuncAppWithMicroServices;

public class BlobStorageTrigger
{
    private readonly ILogger<BlobStorageTrigger> _logger;

    public BlobStorageTrigger(ILogger<BlobStorageTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(BlobStorageTrigger))]
    public async Task Run([BlobTrigger("videos/{name}", Connection = "AzureStorageConnection")] Stream stream, string name)
    {
        if (stream == null)
        {

        }

        else
        {

        }
       Console.WriteLine($"Azure Blob Storage uploaded successfully.:");
    }
}