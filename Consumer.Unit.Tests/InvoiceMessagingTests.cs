using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit.Testing;
using MessageContracts;
using Xunit;

namespace PaymentMicroservice.Unit.Tests
{
    public class InvoiceMessagingTests
    {
        [Fact]
        public async Task Verify_InvoiceCreatedMessage_Consumed()
        {
            var inMemoryBus = new InMemoryTestHarness();
            var invoiceCreatedConsumer = inMemoryBus.Consumer<InvoiceCreatedConsumer>();

            await inMemoryBus.Start();

            try
            {
                var publishedInvoiceCreated = new InvoiceCreated()
                {
                    MessageId = Guid.NewGuid(),
                    InvoiceNumber = 100536800
                };


                await inMemoryBus.Bus.Publish<InvoiceCreated>(publishedInvoiceCreated);

                //1.verify endpoint consumed the message
                var inMemoryBusConsumedResult = await inMemoryBus.Consumed.Any<InvoiceCreated>();
                inMemoryBusConsumedResult.Should().BeTrue();

                //2.verify the real consumer consumed the message
                var invoiceCreatedConsumerResult = await invoiceCreatedConsumer.Consumed.Any<InvoiceCreated>();
                invoiceCreatedConsumerResult.Should().BeTrue();

                //3.verify there was only one message published
                inMemoryBus.Published.Select<InvoiceCreated>().Count().Should().Be(1);

                //4.ensure consumed message is published message
                var consumedInvoiceCreated = inMemoryBus.Published.Select<InvoiceCreated>()
                    .First().Context.Message;

                consumedInvoiceCreated.MessageId.Should().Be(publishedInvoiceCreated.MessageId);
                consumedInvoiceCreated.InvoiceNumber.Should().Be(publishedInvoiceCreated.InvoiceNumber);
            }

            finally
            {
                await inMemoryBus.Stop();
            }
        }
    }
}