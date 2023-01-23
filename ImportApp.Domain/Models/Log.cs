namespace ImportApp.Domain.Models;

public partial class Log : BaseModel
{
    public string? Action { get; set; }

    public DateTime Created { get; set; }

    public int CreatedYear { get; set; }
}
