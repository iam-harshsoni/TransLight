using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class ProductRawMaterial
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid RawMaterialId { get; set; }

    public decimal Qty { get; set; }

    public Guid? UnitId { get; set; }

    public int Type { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Product RawMaterial { get; set; } = null!;

    public virtual Unit? Unit { get; set; }
}
