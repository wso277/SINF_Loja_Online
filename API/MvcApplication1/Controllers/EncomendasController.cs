using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class EncomendasController : ApiController
    {
        // GET api/encomendas/5
        [HttpGet]
        public Lib_Primavera.Models.EncomendaExtended Get(string id)
        {
            Lib_Primavera.Models.EncomendaExtended enc = Lib_Primavera.EncomendasHelper.GetEncomenda(id);
            if (enc == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return enc;
            }
        }

        // GET api/encomendas
        [HttpGet]
        public IEnumerable<Lib_Primavera.Models.EncomendaExtended> GetByClient([FromUri]string CodigoCliente)
        {
            return Lib_Primavera.EncomendasHelper.GetEncomendasCliente(CodigoCliente);
        }

        // PUT api/encomendas
        public HttpResponseMessage Put(Lib_Primavera.Models.Encomenda encomenda)
        {
            Lib_Primavera.Models.Resposta resposta = new Lib_Primavera.Models.Resposta();

            resposta = Lib_Primavera.EncomendasHelper.InsereEncomenda(encomenda);

            if (resposta.Codigo == 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        /*
        // PUT api/encomendas/5
        public void Put(int id, [FromBody]string value)
        {
        }
        */
    }
}
