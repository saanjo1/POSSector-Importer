﻿namespace ImportApp.Domain.Models;

public partial class Customer : BaseModel
{
    public string? ClientNumber { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? TaxNumber { get; set; }

    public string? CityAndPostCode { get; set; }

    public string? BankNumber { get; set; }

    public string? IdNumber { get; set; }

    public decimal Discount { get; set; }

    public bool IsDeleted { get; set; }

    public string? ExtraNote { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
