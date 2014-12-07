using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class Encomenda
    {
        public List<Models.LinhaEncomenda> LinhasEncomenda { get; set; }
        public string Entidade { get; set; }
    }
}