using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio
{
    public class Venta
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public Cliente Cliente { get; set; }
        public Usuario Vendedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}