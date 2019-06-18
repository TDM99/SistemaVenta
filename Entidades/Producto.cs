using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Entidades
{
   public class Producto
    {
        private int _id;
        private Categoria categoria;
        private string _nombre;
        private string _descripcion;
        private double _stock;
        private double _precioCompra;
        private double _precioVenta;
        private DateTime _fechaVencimineto;
        private byte[] _imagen;

        public int Id { get => _id; set => _id = value; }
        public Categoria Categoria { get => categoria; set => categoria = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        public double Stock { get => _stock; set => _stock = value; }
        public double PrecioCompra { get => _precioCompra; set => _precioCompra = value; }
        public double PrecioVenta { get => _precioVenta; set => _precioVenta = value; }
        public DateTime FechaVencimineto { get => _fechaVencimineto; set => _fechaVencimineto = value; }
        public byte[] Imagen { get => _imagen; set => _imagen = value; }

        public Producto()
        {

        }
        public Producto(int id,Categoria categoria,string nombre,string descripcion,double stock,double precioCompra,
            double precioventa,DateTime fechaVencimiento,byte[] imagen)
        {
            Id = id;
            Categoria = categoria;
            Nombre = nombre;
            Descripcion = descripcion;
            Stock = stock;
            PrecioCompra = precioCompra;
            PrecioVenta = precioventa;
            FechaVencimineto = fechaVencimiento;
            Imagen = imagen;
        }
    }
}
