using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class ArticleGood : BaseModel
{

    public decimal Quantity { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }

    public Guid? ArticleId { get; set; }

    public Guid? GoodId { get; set; }

    public virtual Good? Good { get; set; }
}
