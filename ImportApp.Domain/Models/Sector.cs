namespace ImportApp.Domain.Models;

public partial class Sector : BaseModel
{

    public string? Name { get; set; }

    public bool Deleted { get; set; }

    public int Order { get; set; }

    public virtual ICollection<Table> Tables { get; } = new List<Table>();
}
