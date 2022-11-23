using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class StationArticle
{
    public Guid StationId { get; set; }

    public Guid ArticleId { get; set; }

    public virtual Station Station { get; set; } = null!;
}
