using MassTransit;
using MessageContracts;

namespace Client;

public class InvoiceCreatedConsumer:IConsumer<InvoiceCreated>
{
    public async Task Consume(ConsumeContext<InvoiceCreated> context)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"Invoice with number:{context.Message.InvoiceNumber} was created");
        });
    }
}