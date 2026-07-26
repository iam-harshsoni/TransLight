using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class ProductCategory
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Active { get; set; }

    public virtual ICollection<PackingMaterial> PackingMaterials { get; set; } = new List<PackingMaterial>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
}
