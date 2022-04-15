// See https://aka.ms/new-console-template for more information

using MassTransit;
using PaymentMicroservice;

Console.Title = "PaymentMicroservice";
var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host("localhost");
    config.ReceiveEndpoint("payment-service", option =>
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

Console.WriteLine("Payment Micro-service Now Listening");
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