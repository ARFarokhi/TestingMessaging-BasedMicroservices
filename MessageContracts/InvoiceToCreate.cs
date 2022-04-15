namespace MessageContracts;

public class InvoiceToCreate
{
    public InvoiceToCreate()
    {
        InvoiceItems = new List<InvoiceItems>();
    }
    public int CustomerNumber { get; set; }
    public List<InvoiceItems> InvoiceItems { get; set; }
}