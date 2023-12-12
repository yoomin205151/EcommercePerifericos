using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class Producto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Activo { get; set; }

    public int? Stock { get; set; }

    public int? IdCategoria { get; set; }

    public int? Precio { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<CarritoPc> CarritoPcs { get; set; } = new List<CarritoPc>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }
}
