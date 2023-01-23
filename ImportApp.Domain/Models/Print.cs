namespace ImportApp.Domain.Models;

public partial class Print : BaseModel
{

    public string? Text { get; set; }

    public string? Type { get; set; }

    public DateTime Created { get; set; }
}
