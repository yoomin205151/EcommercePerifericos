using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class Origen
{
    public int Id { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
