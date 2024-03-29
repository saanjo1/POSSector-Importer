﻿namespace ImportApp.Domain.Models;

public partial class Article : BaseModel
{

    public bool Deleted { get; set; }

    public byte[]? Image { get; set; }

    public string? Name { get; set; }

    public string? Tag { get; set; }

    public int ArticleNumber { get; set; }

    public int Order { get; set; }

    public decimal Price { get; set; }

    public Guid? SubCategoryId { get; set; }

    public string? BarCode { get; set; }

    public string? Code { get; set; }

    public decimal ReturnFee { get; set; }

    public int FreeModifiers { get; set; }

    public virtual SubCategory? SubCategory { get; set; }
}
