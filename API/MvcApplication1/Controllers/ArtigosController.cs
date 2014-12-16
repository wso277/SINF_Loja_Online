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
    public class ArtigosController : ApiController
    {

        // GET api/artigos/acao
        public Lib_Primavera.Models.HomepageArticles Get(string id)
        {
            Lib_Primavera.Models.HomepageArticles homepage = new HomepageArticles();

            if (id == "homepage")
                homepage = Lib_Primavera.ArtigoHelper.homepage();
            else
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (homepage == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return homepage;
            }
        }

        
        // GET api/artigos
        public IEnumerable<Lib_Primavera.Models.ArtigoShort> Get(int page = 0, bool promocao = false, int precoLimiteSuperior = 9999, int precoLimiteInferior = 0, int ecraLimiteSuperior = 12, int ecraLimiteInferior = 0, string marca = "all", string SO ="all")
        {
            return Lib_Primavera.ArtigoHelper.ListaArtigos(page, promocao, precoLimiteSuperior, precoLimiteInferior, ecraLimiteSuperior, ecraLimiteInferior, marca, SO);
        }

        


    }
}
