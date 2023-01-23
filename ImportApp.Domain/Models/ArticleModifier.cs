namespace ImportApp.Domain.Models;

public partial class ArticleModifier : BaseModel
{

    public Guid? ModifierId { get; set; }

    public Guid? ArticleId { get; set; }
}
