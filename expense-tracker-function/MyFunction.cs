using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;

public static class MyFunction
{
    [FunctionName("MyFunction")]
    public static async Task RunAsync(
        [ServiceBusTrigger("daily-expenses-queue", Connection = "ServiceBusConnectionString")] string inputMessage,
        [SignalR(HubName = "dailyexpenses")] IAsyncCollector<SignalRMessage> signalRMessages,
        ILogger log)
    {
        log.LogInformation($"Received Service Bus message: {inputMessage}");

        // Process the message and perform your custom logic here

        await signalRMessages.AddAsync(new SignalRMessage
        {
            Target = "ExpenseRegistered",
            Arguments = new[] { inputMessage }
        });
    }
}
