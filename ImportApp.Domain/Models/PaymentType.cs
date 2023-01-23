namespace ImportApp.Domain.Models;

public partial class PaymentType : BaseModel
{

    public int Order { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();
}
