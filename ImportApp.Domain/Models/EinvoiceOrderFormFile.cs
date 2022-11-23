using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class EinvoiceOrderFormFile : BaseModel
{

    public byte[]? GzippedContents { get; set; }

    public virtual ICollection<Einvoice> Einvoices { get; } = new List<Einvoice>();
}
