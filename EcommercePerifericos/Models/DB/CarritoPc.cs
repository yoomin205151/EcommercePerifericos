using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class CarritoPc
{
    public int Id { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public int? Preciounitario { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
