// See https://aka.ms/new-console-template for more information

using InvoiceMicroservice;
using MassTransit;

//producer/consumer (publisher/subscriber)

Console.Title = "InvoiceMicroservice";
var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host("localhost");
    config.ReceiveEndpoint("invoice-service", option =>
    {
        option.UseInMemoryOutbox();
        option.Consumer<InvoiceToCreateConsumer>(configure =>
        {
            configure.UseMessageRetry(retryConfig =>
                retryConfig.Interval(5, new TimeSpan(0, 0, 10)));
        });
    });
});

//var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync();//StartAsync(source.Token);

Console.WriteLine("Invoice Micro-service Now Listening");
Console.ReadLine();
//try
//{
//    while (true)
//    {
//        //sit in while loop listening for messages
//        await Task.Delay(100);
//    }
//}
//finally
//{
//    await busControl.StopAsync();
//    Console.WriteLine(" --- bus stopped --- ");
//}

