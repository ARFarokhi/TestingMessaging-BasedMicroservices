// See https://aka.ms/new-console-template for more information

using Client;
using MassTransit;
using MessageContracts;

Console.Title = "TestClient";

Console.WriteLine("Waiting while consumers initialize.");
await Task.Delay(3000); //because the consumers need to start first

var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host("localhost");
    config.ReceiveEndpoint("invoice-service-created", option =>
    {
        option.UseInMemoryOutbox();
        option.Consumer<InvoiceCreatedConsumer>(configure =>
        {
            configure.UseMessageRetry(retryConfig =>
                retryConfig.Interval(5, new TimeSpan(0, 0, 10)));
        });
    });
});

await busControl.StartAsync();

var keyCount = 0;
try
{
    Console.WriteLine("Enter any key to send an invoice request or Q to quit.");
    while (Console.ReadKey(true).Key != ConsoleKey.Q &&
           Console.ReadKey(true).Key != ConsoleKey.F5)
    {
        keyCount++;
        await SendRequestForInvoiceCreation(busControl);
        Console.WriteLine($"Enter any key to send an invoice request or Q to quit. { keyCount}");
    }
}
finally
{
    await busControl.StopAsync();
}

static async Task SendRequestForInvoiceCreation(IBusControl busControl)
{
    var rnd = new Random();
    await busControl.Publish<InvoiceToCreate>(new InvoiceToCreate()
    {
        CustomerNumber = rnd.Next(1000, 9999),
        InvoiceItems = new List<InvoiceItems>()
        {
            new InvoiceItems {Description = "Tables", Price = Math.Round(rnd.NextDouble() * 100, 2)},
            new InvoiceItems {Description = "Chairs", Price = Math.Round(rnd.NextDouble() * 100, 2)}
        }
    });
}