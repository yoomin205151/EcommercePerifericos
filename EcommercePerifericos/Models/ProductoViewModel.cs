namespace EcommercePerifericos.Models
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Nombre { get; set; }
        public int? Stock { get; set; }
        public string Categoria { get; set; } 
        public int? IdCategoria { get; set; }
        public int? Precio { get; set; }
        public string Activo { get; set; }
        public string Detalle { get; set; }
        public int? PrecioOferta { get; set; }

        public ProductoViewModel(int id, string img, string nombre, int? stock, string categoria, int? idCategoria, int? precio, string activo, string detalle, int? precioOferta)
        {
            Id = id;
            Img = img;
            Nombre = nombre;
            Stock = stock;
            Categoria = categoria;
            IdCategoria = idCategoria;
            Precio = precio;
            Activo = activo;
            Detalle = detalle;
            PrecioOferta = precioOferta;
        }

        
    }
}
