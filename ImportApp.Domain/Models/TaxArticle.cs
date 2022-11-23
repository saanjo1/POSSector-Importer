using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class TaxArticle
{
    public Guid TaxId { get; set; }

    public Guid ArticleId { get; set; }

    public virtual Taxis Tax { get; set; } = null!;
}
