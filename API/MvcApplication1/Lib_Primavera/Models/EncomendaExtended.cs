using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class EncomendaExtended
    {
        public List<Models.LinhaEncomendaExtended> LinhasEncomendaExtended { get; set; }
        public string Entidade { get; set; }
        public DateTime Data { get; set; }
        public int NumDoc { get; set; }
        public String Estado { get; set; }
    }
}