using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Marca Marca { get; set; }
        public Categoría Categoria { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public decimal PorcentajeGanancia { get; set; }
    }
}