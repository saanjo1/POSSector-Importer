namespace ImportApp.Domain.Models;

public partial class InvoiceItemModifier : BaseModel
{

    public decimal PriceWithoutDiscount { get; set; }

    public Guid? ModifierId { get; set; }

    public Guid? InvoiceItemId { get; set; }

    public virtual InvoiceItem? InvoiceItem { get; set; }
}
