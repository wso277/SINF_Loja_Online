using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class Artigo
    {
        public string CodigoArtigo { get; set; }
        public string Nome { get; set; }
        public float StockAtual { get; set; }


        /** Preço **/
        public float PVP { get; set; }
        public float Desconto { get; set; }

        /* Especificacoes do joao */
         //todo



    }
}