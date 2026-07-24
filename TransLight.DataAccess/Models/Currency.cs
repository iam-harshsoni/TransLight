using System;
using System.Collections.Generic;

namespace TransLight.DataAccess.Models;

public partial class Currency
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
