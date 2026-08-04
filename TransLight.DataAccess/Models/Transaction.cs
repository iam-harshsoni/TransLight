using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CompanySiteId { get; set; }

    public Guid PartyId { get; set; }

    public Guid PartySiteId { get; set; }

    public string Id2Format { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int Type { get; set; }

    public int TransactionType { get; set; }

    public Guid CurrencyId { get; set; }

    public decimal ExchangeRate { get; set; }

    public int DeliveryType { get; set; }

    public int Cancel { get; set; }

    public string Remarks { get; set; } = null!;

    public decimal BasicAmt { get; set; }

    public decimal GstAmt { get; set; }

    public decimal RoundOffAmt { get; set; }

    public decimal TotalAmt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual CompanySite CompanySite { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;
}
