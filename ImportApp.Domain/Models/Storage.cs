using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class Storage : BaseModel
{

    public string? Name { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Category> Categories { get; } = new List<Category>();

    public virtual ICollection<InventoryDocument> InventoryDocuments { get; } = new List<InventoryDocument>();

    public virtual ICollection<InventoryItemBasis> InventoryItemBases { get; } = new List<InventoryItemBasis>();

    public virtual ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
}
