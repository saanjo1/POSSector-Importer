﻿namespace ImportApp.Domain.Models;

public partial class Table : BaseModel
{
    public string? Name { get; set; }

    public int Order { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public double Rotation { get; set; }

    public int Type { get; set; }

    public int State { get; set; }

    public bool Deleted { get; set; }

    public Guid? WaiterId { get; set; }

    public Guid? SectorId { get; set; }

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();

    public virtual Sector? Sector { get; set; }

    public virtual User? Waiter { get; set; }
}
