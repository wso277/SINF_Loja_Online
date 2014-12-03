﻿using System;
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

        // GET api/artigos/id 
        public Artigo Get(string id)
        {
            Lib_Primavera.Models.Artigo artigo = Lib_Primavera.ArtigoHelper.GetArtigo(id);
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
        public IEnumerable<Lib_Primavera.Models.ArtigoShort> Get()
        {
            return Lib_Primavera.ArtigoHelper.ListaArtigos();
        }


    }
}
