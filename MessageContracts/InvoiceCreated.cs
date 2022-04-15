namespace MessageContracts;


public class InvoiceCreated
{
    public Guid MessageId { get; set; }
    public int InvoiceNumber { get; set; }
    public InvoiceToCreate InvoiceData { get; set; }
}