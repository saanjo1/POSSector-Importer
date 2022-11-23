using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class Good : BaseModel
{
    public string? Name { get; set; }

    public Guid? UnitId { get; set; }

    public decimal LatestPrice { get; set; }

    public decimal? Volumen { get; set; }

    public decimal? Refuse { get; set; }

    public virtual ICollection<ArticleGood> ArticleGoods { get; } = new List<ArticleGood>();

    public virtual ICollection<InventoryItemBasis> InventoryItemBases { get; } = new List<InventoryItemBasis>();

    public virtual MeasureUnit? Unit { get; set; }
}
