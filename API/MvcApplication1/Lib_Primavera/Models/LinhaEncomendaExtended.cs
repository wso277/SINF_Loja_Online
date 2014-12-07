using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class LinhaEncomendaExtended : LinhaEncomenda
    {
        public string DescricaoArtigo { get; set; }
        public double Desconto { get; set; }
        public double PrecoUnitario { get; set; }
        public double TotalILiquido { get; set; }
        public double TotalLiquido { get; set; }
    }
}