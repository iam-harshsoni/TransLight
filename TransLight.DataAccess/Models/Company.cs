using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Company
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? City { get; set; }

    public int? Pincode { get; set; }

    public string? State { get; set; }

    public string? Contact { get; set; }

    public int? AccountContact { get; set; }

    public string? Email { get; set; }

    public string? BlDraftEmail { get; set; }

    public string? Website { get; set; }

    public string? AccountEmail { get; set; }

    public string? PanNo { get; set; }

    public string? TanNo { get; set; }

    public string? ChaNo { get; set; }

    public string? ChaLicenseNo { get; set; }

    public string? MtoRegiNo { get; set; }

    public string? CinNo { get; set; }

    public string? GstNo { get; set; }

    public string? MsmeNo { get; set; }

    public string? Bank { get; set; }

    public string? AccountNo { get; set; }

    public string? IfscCode { get; set; }

    public string? Branch { get; set; }

    public string? UsdBank { get; set; }

    public string? UsdAccountNo { get; set; }

    public string? UsdIfscCode { get; set; }

    public string? UsdBranch { get; set; }

    public string? Remarks { get; set; }

    public string? Uuid { get; set; }

    public string? TallyName { get; set; }

    public string? Guid { get; set; }

    public string? Logo { get; set; }

    public string? Signature { get; set; }

    public string? Stamp { get; set; }

    public string? EinvoiceUsername { get; set; }

    public string? EinvoicePassword { get; set; }

    public string? EinvoiceAuthToken { get; set; }

    public DateTime? EinvoiceTokenExpiry { get; set; }

    public string? TermsConditions { get; set; }

    public string? ThemeColor { get; set; }

    public virtual ICollection<CompanySite> CompanySites { get; set; } = new List<CompanySite>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
