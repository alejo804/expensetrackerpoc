using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace TrackExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ExpenseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostExpense([FromBody] Expense expense)
        {
            var connectionString = _configuration["AzureServiceBus:ConnectionString"];
            var queueName = "daily-expenses-queue"; // Replace with your Azure Service Bus queue or topic name
            System.Console.WriteLine(connectionString);
            System.Console.WriteLine(connectionString);

            var expenseSerialized = JsonConvert.SerializeObject(expense);

            var message = new Message(Encoding.UTF8.GetBytes(expenseSerialized));

            QueueClient client = null;
            try
            {
                client = new QueueClient(connectionString, queueName);
                await client.SendAsync(message);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok();
            }
            finally
            {
                if (client != null)
                {
                    await client.CloseAsync();
                }
            }
        }
    }
}
