using System;
using System.Collections.Generic;

namespace ImportApp.Domain.Models;

public partial class Primka
{
    public string? Item { get; set; }

    public string? Naziv { get; set; }

    public string? Gender { get; set; }

    public string? Barcode { get; set; }

    public double? SoPrice { get; set; }

    public double? Cenakune { get; set; }
}
