using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public int Type { get; set; }

    public string Name { get; set; } = null!;

    public string? Make { get; set; }

    public string? Pack { get; set; }

    public Guid? UnitId { get; set; }

    public decimal Rate { get; set; }

    public decimal? Gst { get; set; }

    public string Hsn { get; set; } = null!;

    public int? Msl { get; set; }

    public string? TallyNameSales { get; set; }

    public string? TallyNamePurchase { get; set; }

    public int Active { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<ProductRawMaterial> ProductRawMaterialProducts { get; set; } = new List<ProductRawMaterial>();

    public virtual ICollection<ProductRawMaterial> ProductRawMaterialRawMaterials { get; set; } = new List<ProductRawMaterial>();

    public virtual Unit? Unit { get; set; }
}
