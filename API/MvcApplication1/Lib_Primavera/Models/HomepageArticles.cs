using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class HomepageArticles
    {
        public ArtigoShowcase novidade { get; set; }
        public ArtigoShowcase oportunidade { get; set; }
        public ArtigoShowcase maisvendido { get; set; }
        public ArtigoShowcase stock { get; set; }
   
    }
}