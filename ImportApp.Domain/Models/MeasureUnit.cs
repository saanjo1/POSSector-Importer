namespace ImportApp.Domain.Models;

public partial class MeasureUnit : BaseModel
{

    public string? Name { get; set; }

    public virtual ICollection<Good> Goods { get; } = new List<Good>();
}
