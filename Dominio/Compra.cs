using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio
{
    public class Compra
    {
        public int Id { get; set; }
        public Proveedor Proveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}