using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class Taxis : BaseModel
{
    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string? Name { get; set; }

    public decimal Value { get; set; }

    public bool IsDeleted { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TaxArticle> TaxArticles { get; } = new List<TaxArticle>();

    public virtual ICollection<Rule> Rules { get; } = new List<Rule>();
}
