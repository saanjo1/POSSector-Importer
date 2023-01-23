namespace ImportApp.Domain.Models;

public partial class SubCategory : BaseModel
{

    public int Order { get; set; }

    public string? Printer { get; set; }

    public string? Name { get; set; }

    public bool Deleted { get; set; }

    public Guid? StorageId { get; set; }

    public Guid? CategoryId { get; set; }

    public string? Tag { get; set; }

    public string? ExtraPrinter1 { get; set; }

    public string? ExtraPrinter2 { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Novi> Novis { get; } = new List<Novi>();

    public virtual Storage? Storage { get; set; }
}
