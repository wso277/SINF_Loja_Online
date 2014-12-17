using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;
//
//

namespace MvcApplication1.Lib_Primavera
{
    public class ArtigoHelper
    {
        static string COMPANHIA = InformacaoEmpresa.COMPANHIA;
        static string USER = InformacaoEmpresa.USER;
        static string PASS = InformacaoEmpresa.PASS;
        static string ARMAZEM = "ARM01";

        public static Lib_Primavera.Models.Artigo GetArtigo(string codArtigo)
        {
            GcpBEArtigo objArtigo = new GcpBEArtigo();
            IGcpBSArtigos camposUser;
            GcpBEArtigoMoeda preco = new GcpBEArtigoMoeda();
            Models.Artigo myArt = new Models.Artigo();

                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {
                    if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false){
                        return null;
                    }

                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    camposUser = PriEngine.Engine.Comercial.Artigos;

                    //Codigo do Artigo
                    myArt.CodigoArtigo = objArtigo.get_Artigo();

                    //Nome
                    myArt.Nome = objArtigo.get_Descricao();

                    //Stock
                    myArt.StockAtual = getStockAtual(codArtigo);

                    //PVP
                    preco = PriEngine.Engine.Comercial.ArtigosPrecos.Edita(codArtigo, "EUR", "UN");
                    myArt.PVP = (float) preco.get_PVP1();
                    
                    //Desconto
                    myArt.Desconto = objArtigo.get_Desconto();

                    //Marca
                    myArt.Marca = PriEngine.Engine.Comercial.Marcas.DaValorAtributo(objArtigo.get_Marca(), "descricao");

                    //Nome SO
                    myArt.NomeSistemaOperativo = PriEngine.Engine.Comercial.Familias.DaDescricao(objArtigo.get_Familia());
                   
                    //Versao SO
                    myArt.VersaoSistemaOperativo = objArtigo.get_SubFamilia();

                    //Tamanho Ecra
                    myArt.TamanhoEcra = (float) camposUser.DaValorAtributo(codArtigo, "CDU_ECRA");

                    //RAM
                    myArt.RAM = camposUser.DaValorAtributo(codArtigo, "CDU_RAM");

                    //CPU
                    myArt.CPU = camposUser.DaValorAtributo(codArtigo, "CDU_CPU");

                    //Peso
                    myArt.Peso = camposUser.DaValorAtributo(codArtigo, "CDU_PESO");

                    //Camara
                    myArt.CamaraTraseira = camposUser.DaValorAtributo(codArtigo, "CDU_CAMARA");

                    //Armazenamento
                    myArt.Armazenamento = camposUser.DaValorAtributo(codArtigo, "CDU_ARMAZENAMENTO");

                    //Autonomia
                    myArt.Autonomia = (float) camposUser.DaValorAtributo(codArtigo, "CDU_AUTONOMIA");

                    //GPU
                    myArt.GPU = camposUser.DaValorAtributo(codArtigo, "CDU_GPU");

                    //FOTOURL
                    myArt.fotoURL = camposUser.DaValorAtributo(codArtigo, "CDU_FOTO");

                    //Data de Lancamento
                    DateTime d = camposUser.DaValorAtributo(codArtigo, "CDU_Lancamento");
                    myArt.Lancamento = d.ToShortDateString();
  

                    return myArt;
                }
                else
                {

                    return null;
                }
            }




        public static IEnumerable<Models.ArtigoShort> ListaArtigos(int page, bool promocao, int precoLimiteSuperior, int precoLimiteInferior, int ecraLimiteSuperior, int ecraLimiteInferior, string marca, string SO)
        {
            ErpBS objMotor = new ErpBS();

            StdBELista objList;

            Models.ArtigoShort art = new Models.ArtigoShort();
            List<Models.ArtigoShort> listArts = new List<Models.ArtigoShort>();

            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim()){
                   
                    art = new Models.ArtigoShort();
                    DateTime hoje = DateTime.Today;

                    //Codigo do Artigo
                    art.CodigoArtigo = objList.Valor("artigo");

                    GcpBEArtigo objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(art.CodigoArtigo);
                    IGcpBSArtigos camposUser = PriEngine.Engine.Comercial.Artigos;

                    if (objArtigo.get_Desconto() == 0 && promocao == true) {
                        objList.Seguinte();
                        continue;
                    }
                        

                    //Nome
                    art.Nome = objArtigo.get_Descricao();

                    //Stock
                    art.StockAtual = getStockAtual(art.CodigoArtigo);

                    //PVP
                    GcpBEArtigoMoeda preco = PriEngine.Engine.Comercial.ArtigosPrecos.Edita(art.CodigoArtigo, "EUR", "UN");
                    art.PVP = (float)preco.get_PVP1();

                    if (art.PVP > precoLimiteSuperior || art.PVP < precoLimiteInferior){
                        objList.Seguinte();
                        continue;
                    }

                    //Desconto
                    art.Desconto = objArtigo.get_Desconto();

                    //Nome SO
                    art.NomeSistemaOperativo = PriEngine.Engine.Comercial.Familias.DaDescricao(objArtigo.get_Familia());

                    if (SO != "all")
                    {
                        if (SO != art.NomeSistemaOperativo)
                        {
                            objList.Seguinte();
                            continue;
                        }
                    }

                    //Marca
                    art.Marca = PriEngine.Engine.Comercial.Marcas.DaValorAtributo(objArtigo.get_Marca(), "descricao");
                    if (marca != "all")
                    {
                        if (marca != art.Marca)
                        {
                            objList.Seguinte();
                            continue;
                        }
                    }

                    //Tamanho Ecra
                    art.TamanhoEcra = (float)camposUser.DaValorAtributo(art.CodigoArtigo, "CDU_ECRA");

                    if (art.TamanhoEcra > ecraLimiteSuperior || art.TamanhoEcra < ecraLimiteInferior)
                    {
                        objList.Seguinte();
                        continue;
                    }

                    //CPU
                    art.CPU = camposUser.DaValorAtributo(art.CodigoArtigo, "CDU_CPU");

                    //Armazenamento
                    art.Armazenamento = camposUser.DaValorAtributo(art.CodigoArtigo, "CDU_ARMAZENAMENTO");

                    //FOTOURL
                    art.fotoURL = camposUser.DaValorAtributo(art.CodigoArtigo, "CDU_FOTO");

                    //Data de Lancamento
                    DateTime d= camposUser.DaValorAtributo(art.CodigoArtigo, "CDU_Lancamento");
                    art.Lancamento = d.ToShortDateString();

                    //Avaliacao
                    int anos = hoje.Year - d.Year;
                    float peso_anos = 0;

                    if (anos >= 3)
                        peso_anos = (Math.Abs(3 - anos) + 1);
                    else if (anos < 3)
                        peso_anos = (anos + 1);

                                      
                    art.avaliacao = 100 * art.Desconto + 400 / peso_anos +  art.StockAtual;
  
                    listArts.Add(art);
                    objList.Seguinte();
                }


                //Ordena
                List<Models.ArtigoShort> listaOrdenada = listArts.OrderByDescending(a => a.avaliacao).ToList();

                //restringe pelas paginas
                List<Models.ArtigoShort> resultado = new List<Models.ArtigoShort>();

                int artigos_por_pagina = 4;
                int offset = artigos_por_pagina * page;

                for (int j = 0; j < artigos_por_pagina; j++)
                {
                    if (j + offset < listaOrdenada.Count())
                        resultado.Add(listaOrdenada.ElementAt(offset + j));
                }

                    return resultado;

            }
            else
            {
                return null;

            }
        }

        /** Retorna o produto com a data de lancamento mais recente **/
        private static Models.ArtigoShowcase novidade()
        {
            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {
                String query = "SELECT TOP 1 Artigo as CodigoArtigo, CDU_FOTO as fotoURL, CDU_Lancamento as data FROM Artigo ORDER BY data DESC;";

                StdBELista objList = PriEngine.Engine.Consulta(query);
                Models.ArtigoShowcase art = new Models.ArtigoShowcase();

                if (objList.Vazia() == false)
                {

                    art.CodigoArtigo = objList.Valor("CodigoArtigo");
                    art.fotoURL = objList.Valor("fotoURL");

                    return art;
                }
                else { return null; }

            }
            else { return null; }
        }

        /** Retorna o produto com maior PROMOCAO **/
        private static Models.ArtigoShowcase oportunidade()
        {
            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {
                String query = "SELECT TOP 1 Artigo as CodigoArtigo, CDU_FOTO as fotoURL, CDU_Lancamento as data FROM Artigo WHERE desconto > 0 ORDER BY desconto, data DESC;";

                StdBELista objList = PriEngine.Engine.Consulta(query);
                Models.ArtigoShowcase art = new Models.ArtigoShowcase();

                if (objList.Vazia() == false)
                {

                    art.CodigoArtigo = objList.Valor("CodigoArtigo");
                    art.fotoURL = objList.Valor("fotoURL");

                    return art;
                }
                else { return null; }

            }
            else { return null; }
        }

        private static Models.ArtigoShowcase maisvendido()
        {
            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {
                String query = "SELECT TOP 1 Artigo.Artigo as CodigoArtigo, CDU_FOTO as fotoURL, Sum(LinhasDoc.Quantidade) as quantidade " +
                "FROM Artigo "+
                "LEFT JOIN LinhasDoc ON Artigo.Artigo = LinhasDoc.Artigo "+
                "LEFT JOIN LinhasDocStatus ON LinhasDocStatus.IdLinhasDoc = LinhasDoc.Id " +
                "WHERE LinhasDocStatus.EstadoTrans='P' GROUP BY Artigo.Artigo, Artigo.CDU_FOTO ORDER BY quantidade DESC;";



                StdBELista objList = PriEngine.Engine.Consulta(query);
                Models.ArtigoShowcase art = new Models.ArtigoShowcase();

                if (objList.Vazia() == false)
                {

                    art.CodigoArtigo = objList.Valor("CodigoArtigo");
                    art.fotoURL = objList.Valor("fotoURL");
                    double quantidade = objList.Valor("quantidade");

                    return art;
                }
                else { return null; }

            }
            else { return null; }
        }

        private static Models.ArtigoShowcase stock()
        {
            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {
                String query = "SELECT Artigo.Artigo as CodigoArtigo, CDU_FOTO as fotoURL, ArtigoArmazem.stkactual as stock FROM Artigo, ArtigoArmazem WHERE Artigo.Artigo = ArtigoArmazem.Artigo ORDER BY stock DESC;";

                StdBELista objList = PriEngine.Engine.Consulta(query);
                Models.ArtigoShowcase art = new Models.ArtigoShowcase();

                if (objList.Vazia() == false)
                {

                    art.CodigoArtigo = objList.Valor("CodigoArtigo");
                    art.fotoURL = objList.Valor("fotoURL");

                    return art;
                }
                else { return null; }

            }
            else { return null; }
        }

        private static float getStockAtual(string codArtigo)
        {

            IGcpBSArtigosArmazens armazem;
            armazem = PriEngine.Engine.Comercial.ArtigosArmazens;

            return (float) armazem.DaStockDisponivelArtigo(codArtigo);
        }

        public static Models.HomepageArticles homepage()
        {
            Models.HomepageArticles home = new Models.HomepageArticles();

            home.maisvendido = maisvendido();
            home.novidade = novidade();
            home.oportunidade = oportunidade();
            home.stock = stock();

            return home;
        }
    }
}
