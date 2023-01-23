﻿namespace ImportApp.Domain.Models;

public partial class Category : BaseModel
{

    public int Order { get; set; }

    public string? Printer { get; set; }

    public string? Name { get; set; }

    public bool Deleted { get; set; }

    public Guid? StorageId { get; set; }

    public string? ExtraPrinter1 { get; set; }

    public string? ExtraPrinter2 { get; set; }

    public virtual Storage? Storage { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
}
