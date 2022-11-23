using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class UserGroup : BaseModel
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
