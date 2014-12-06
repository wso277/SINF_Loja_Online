using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApplication1.Lib_Primavera.Models;


namespace MvcApplication1.Controllers
{
    public class ClientsController : ApiController
    {
        
        // PUT Cliente ** without password **
        public HttpResponseMessage Put(Lib_Primavera.Models.Client cliente)
        {
            Lib_Primavera.Models.Resposta resposta = new Lib_Primavera.Models.Resposta();

            resposta = Lib_Primavera.ClientHelper.InsereCliente(cliente);

            if (resposta.Codigo == 0){
                /*
                var response = Request.CreateResponse(HttpStatusCode.Created, cliente);
                string uri = Url.Link("DefaultApi", new { CodCliente = cliente.CodigoCliente });
                response.Headers.Location = new Uri(uri);
                return response;
                */
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public Client Get(string id)
        {
            Lib_Primavera.Models.Client cliente = Lib_Primavera.ClientHelper.GetCliente(id);
            if (cliente == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return cliente;
            }
        }
    }
}
