using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class RuleItem : BaseModel
{

    public decimal NewPrice { get; set; }

    public Guid? ArticleId { get; set; }

    public Guid? RuleId { get; set; }

    public virtual Rule? Rule { get; set; }
}
