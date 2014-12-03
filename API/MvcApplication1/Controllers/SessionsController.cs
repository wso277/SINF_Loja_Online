using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class SessionsController : ApiController
    {
        // POST login
        public HttpResponseMessage Post(Lib_Primavera.Models.Session session)
        {
            Lib_Primavera.Models.Resposta resposta = new Lib_Primavera.Models.Resposta();

            resposta = Lib_Primavera.SessionsHelper.FazLogin(session);

            if (resposta.Codigo == 0)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, session);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
