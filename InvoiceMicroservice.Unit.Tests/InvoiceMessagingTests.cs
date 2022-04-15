using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit.Testing;
using MessageContracts;
using Xunit;

namespace InvoiceMicroservice.Unit.Tests
{
    public class InvoiceMessagingTests
    {
        [Fact]
        public async Task Verify_InvoiceToCreateCommand_Consumed()
        {
            //Verify that we are receiving and reacting
            //to a command to create an invoice

            var inMemoryBus = new InMemoryTestHarness();
            var invoiceToCreateConsumer = inMemoryBus.Consumer<InvoiceToCreateConsumer>();

            await inMemoryBus.Start();
            try
            {
                var invoiceToCreate = new InvoiceToCreate()
                {
                    CustomerNumber = 19282,
                    InvoiceItems = new List<InvoiceItems>()
                    {
                        new InvoiceItems
                        {
                            Description = "HDD",
                            Price = 1200
                        }
                    }
                };

                //await inMemoryBus.InputQueueSendEndpoint.Send<InvoiceToCreate>(invoiceToCreate);
                await inMemoryBus.InputQueueSendEndpoint.Send<InvoiceToCreate>(invoiceToCreate);

                //1.verify endpoint consumed the message
                var inMemoryBusConsumedResult = await inMemoryBus.Consumed.Any<InvoiceToCreate>();
                inMemoryBusConsumedResult.Should().BeTrue();

                //2.verify the real consumer consumed the message
                var invoiceToCreateConsumerResult = await invoiceToCreateConsumer.Consumed.Any<InvoiceToCreate>();
                invoiceToCreateConsumerResult.Should().BeTrue();

                //3.verify that a new message was published
                //because of the new invoice being created
                inMemoryBus.Published.Select<InvoiceCreated>().Count().Should().Be(1);

                //4.ensure consumed message is published message
                var consumedInvoiceToCreate = inMemoryBus.Consumed.Select<InvoiceToCreate>()
                    .First().Context.Message;

                consumedInvoiceToCreate.CustomerNumber.Should().Be(invoiceToCreate.CustomerNumber);
                consumedInvoiceToCreate.InvoiceItems.Should().BeEquivalentTo(invoiceToCreate.InvoiceItems);

            }
            finally
            {
                await inMemoryBus.Stop();
            }
        }
    }
}