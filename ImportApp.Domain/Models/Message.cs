namespace ImportApp.Domain.Models;

public partial class Message : BaseModel
{

    public DateTime Created { get; set; }

    public bool IsRead { get; set; }

    public int Type { get; set; }

    public string? Title { get; set; }

    public string? Text { get; set; }

    public Guid? FromId { get; set; }

    public Guid? ToId { get; set; }

    public virtual User? From { get; set; }

    public virtual User? To { get; set; }
}
