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
        public Lib_Primavera.Models.ArtigoShowcase Get(string id)
        {
            Lib_Primavera.Models.ArtigoShowcase artigo = new ArtigoShowcase();

            if (id == "novidade")
                artigo = Lib_Primavera.ArtigoHelper.novidade();

            else if (id == "oportunidade")
                artigo = Lib_Primavera.ArtigoHelper.oportunidade();

            else if (id == "maisvendido")
                artigo = Lib_Primavera.ArtigoHelper.maisvendido();

            else if( id == "stock" )
                artigo = Lib_Primavera.ArtigoHelper.stock();

            if (artigo == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return artigo;
            }
        }

        
        // GET api/artigos
        public IEnumerable<Lib_Primavera.Models.ArtigoShort> Get(int page=0, bool promocao=false)
        {
            return Lib_Primavera.ArtigoHelper.ListaArtigos(page, promocao);
        }

        


    }
}
