namespace ImportApp.Domain.Models;

public partial class OrdersPerDate
{
    public DateTime Date { get; set; }

    public int NumberOfOrders { get; set; }
}
