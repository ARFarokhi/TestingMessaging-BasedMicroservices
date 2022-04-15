using MassTransit;
using MessageContracts;

namespace PaymentMicroservice;

public class InvoiceCreatedConsumer : IConsumer<InvoiceCreated>
{
    public async Task Consume(ConsumeContext<InvoiceCreated> context)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"Received message for invoice number:{context.Message.InvoiceNumber}");
        });
    }
}