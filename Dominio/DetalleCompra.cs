using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int IdCompra { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}