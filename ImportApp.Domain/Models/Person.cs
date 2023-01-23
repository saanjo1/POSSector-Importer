﻿namespace ImportApp.Domain.Models;

public partial class Person : BaseModel
{

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumbers { get; set; }

    public Guid? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
