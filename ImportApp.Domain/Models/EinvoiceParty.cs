﻿namespace ImportApp.Domain.Models;

public partial class EinvoiceParty : BaseModel
{

    public string RegistrationName { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    public string? EndpointId { get; set; }

    public string? EndpointIdSchemeId { get; set; }

    public string? PartyIdentificationId { get; set; }

    public string? BusinessBranchName { get; set; }

    public string? PostalAddressStreetName { get; set; }

    public string? PostalAddressCityName { get; set; }

    public string? PostalAddressPostalCode { get; set; }

    public virtual ICollection<Einvoice> EinvoiceReceivers { get; } = new List<Einvoice>();

    public virtual ICollection<Einvoice> EinvoiceSenders { get; } = new List<Einvoice>();
}
