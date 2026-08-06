using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class TransactionDetail
{
    public Guid Id { get; set; }

    public string Vertical { get; set; } = null!;

    public Guid TransactionId { get; set; }

    public string SrNo { get; set; } = null!;

    public Guid ProductId { get; set; }

    public string Description { get; set; } = null!;

    public int Qty { get; set; }

    public Guid UnitId { get; set; }

    public decimal Rate { get; set; }

    public decimal BasicAmt { get; set; }

    public decimal GstPer { get; set; }

    public decimal GstAmt { get; set; }

    public decimal TotalAmt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
