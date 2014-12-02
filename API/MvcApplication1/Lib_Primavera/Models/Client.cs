using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class Client
    {
        /** Identificação do cliente **/
        public string CodigoCliente { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        /** Dados de envio da encomenda **/
        public string Morada { get; set; }
        public string Localidade { get; set; }
        //public string CodDistrito { get; set; }
        public string CodPostal { get; set; }

        /** Dados Fiscais **/
        public string NumContribuinte { get; set; }



    }
}