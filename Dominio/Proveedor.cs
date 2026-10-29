using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Telefono { get; set; }
    }
}
