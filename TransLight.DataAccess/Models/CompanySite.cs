using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class CompanySite
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? City { get; set; }

    public int? Pincode { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public string? GstNo { get; set; }

    public string? LutNo { get; set; }

    public string? EinvoiceUsername { get; set; }

    public string? EwayUsername { get; set; }

    public string? EwayPassword { get; set; }

    public int Active { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
