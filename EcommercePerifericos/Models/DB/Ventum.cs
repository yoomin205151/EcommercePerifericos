using System;
using System.Collections.Generic;

namespace EcommercePerifericos.Models.DB;

public partial class Ventum
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Total { get; set; }

    public int? IdOrigen { get; set; }

    public string? NombreCliente { get; set; }

    public string? Direccion { get; set; }

    public string? CodigoPostal { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Origen? IdOrigenNavigation { get; set; }
}
