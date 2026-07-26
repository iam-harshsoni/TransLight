using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Unit
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<PackingMaterial> PackingMaterials { get; set; } = new List<PackingMaterial>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
}
