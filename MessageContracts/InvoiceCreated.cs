namespace MessageContracts;

public class InvoiceCreated
{
    public int InvoiceNumber { get; set; }
    public InvoiceToCreate InvoiceData { get; set; }
}