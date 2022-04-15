using MassTransit;
using MessageContracts;

namespace InvoiceMicroservice;

public class InvoiceToCreateConsumer : IConsumer<InvoiceToCreate>
{
    public async Task Consume(ConsumeContext<InvoiceToCreate> context)
    {
        var message = context.Message;
        var newInvoiceNumber = new Random().Next(10000, 99999);
        Console.WriteLine($"Creating invoice {newInvoiceNumber} for customer:{ message.CustomerNumber}");
        message.InvoiceItems.ForEach(PrintInvoiceItem);

        Console.WriteLine();
        await PublishInvoiceCreatedMessage(context, newInvoiceNumber);
    }

    private static async Task PublishInvoiceCreatedMessage(ConsumeContext<InvoiceToCreate> context, int invoiceNumber)
    {
        await context.Publish<InvoiceCreated>(new InvoiceCreated()
        {
            MessageId = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            InvoiceData = new InvoiceToCreate()
            {
                CustomerNumber = context.Message.CustomerNumber,
                InvoiceItems = context.Message.InvoiceItems
            }
        });
    }

    private static void PrintInvoiceItem(InvoiceItems item)
    {
        Console.WriteLine($"With items: Price: {item.Price}, Desc:{item.Description}");
    }
}