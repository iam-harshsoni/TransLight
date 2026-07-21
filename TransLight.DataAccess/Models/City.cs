using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class City
{
    public Guid Id { get; set; }

    public Guid StateId { get; set; }

    public string Name { get; set; } = null!;

    public int Pincode { get; set; }

    public string District { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual State State { get; set; } = null!;
}
