using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
