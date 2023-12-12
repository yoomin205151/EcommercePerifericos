using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class Rol
{
    public int Id { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
