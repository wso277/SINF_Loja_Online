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
        static string COMPANHIA = "BELAFLOR";
        static string USER = "";
        static string PASS = "";

        public static Lib_Primavera.Models.Artigo GetArtigo(string codArtigo)
        {
            GcpBEArtigo objArtigo = new GcpBEArtigo();
            GcpBEArtigoMoeda preco = new GcpBEArtigoMoeda();
            Models.Artigo myArt = new Models.Artigo();

                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {

                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    myArt.CodigoArtigo = objArtigo.get_Artigo();
                    myArt.Nome = objArtigo.get_Descricao();
                    myArt.StockAtual = (float) objArtigo.get_StkActual();

                    preco = PriEngine.Engine.Comercial.ArtigosPrecos.Edita(codArtigo, "EUR", "");
                    myArt.PVP = (float) preco.get_PVP1();
                    
                    myArt.Desconto = objArtigo.get_Desconto();
                    //myArt.Marc =

                    return myArt;
                }
                else
                {

                    return null;
                }
            }

        }

        public static Lib_Primavera.Models.Resposta AtualizaCliente(Lib_Primavera.Models.Client cliente)
        {



            Lib_Primavera.Models.Resposta resposta = new Models.Resposta();
            ErpBS objMotor = new ErpBS();

            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodigoCliente) == false)
                    {
                        resposta.Codigo = 1;
                        resposta.Descricao = "O cliente não existe";
                        return resposta;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodigoCliente);
                        objCli.set_EmModoEdicao(true);

                        // Nome
                        if (cliente.Nome != "")
                            objCli.set_Nome(cliente.Nome);

                        //Telefone
                        if (cliente.Telefone != "")
                            objCli.set_Telefone(cliente.Telefone);

                        //Morada
                        if (cliente.Morada != "")
                            objCli.set_Morada(cliente.Morada);

                        //Localidade
                        if (cliente.Localidade != "")
                            objCli.set_Localidade(cliente.Localidade);

                        //CodDistrito
                        if (cliente.CodDistrito != "")
                            objCli.set_Distrito(cliente.CodDistrito); //perguntar ao stor se é o ID do distrito ou a descricao

                        //CodPostal
                        if (cliente.CodPostal != "")
                            objCli.set_CodigoPostal(cliente.CodPostal);

                        //NumContribuinte
                        if (cliente.NumContribuinte != "")
                            objCli.set_NumContribuinte(cliente.NumContribuinte);



                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        resposta.Codigo = 0;
                        resposta.Descricao = "Sucesso";
                        return resposta;
                    }
                }
                else
                {
                    resposta.Codigo = 1;
                    resposta.Descricao = "Erro ao abrir a empresa " + COMPANHIA;
                    return resposta;

                }

            }

            catch (Exception ex)
            {
                resposta.Codigo = 1;
                resposta.Descricao = ex.Message;
                return resposta;
            }

        }






    }
}