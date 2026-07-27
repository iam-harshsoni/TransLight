using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Country
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
