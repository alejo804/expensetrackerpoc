using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;


namespace ExpnenseTrackerApp.Pages
{
    public class RegisterExpenseModel : PageModel
    {
        [BindProperty]
        public ExpenseModel Expense { get; set; }

        public void OnGet()
        {
            Expense = new ExpenseModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Create the expense object
            var expense = new ExpenseModel
            {
                Description = Expense.Description,
                Amount = Expense.Amount
            };

            // Convert the expense object to JSON
            var jsonExpense = JsonConvert.SerializeObject(expense);

            // Create a Service Bus message with the JSON payload
            var message = new Message(Encoding.UTF8.GetBytes(jsonExpense));

            // TODO: Set the appropriate Service Bus connection string and queue name
            var connectionString = "Endpoint=sb://daily-expenses.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9hEsHjVVHDtJ+wH/XuOhvm/khLdM9Eo0v+ASbH6qAqc=";
            var queueName = "daily-expenses-queue";

            // Create a Service Bus client using the connection string
            var client = new QueueClient(connectionString, queueName);

            try
            {
                // Send the message to the Service Bus queue
                await client.SendAsync(message);

                // Close the client and release resources
                await client.CloseAsync();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during message sending
                // TODO: Handle the exception according to your application's requirements
                Console.WriteLine($"Error sending message to Service Bus: {ex.Message}");
                return RedirectToPage("Error");
            }

            return RedirectToPage("Index");
        }
    }
    public class ExpenseModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
