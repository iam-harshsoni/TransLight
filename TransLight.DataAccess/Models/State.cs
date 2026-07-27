using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class State
{
    public Guid Id { get; set; }

    public string Gst { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int UnionTerritory { get; set; }

    public Guid CountryId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
