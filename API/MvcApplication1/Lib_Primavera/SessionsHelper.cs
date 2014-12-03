using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.StdBE800;

namespace MvcApplication1.Lib_Primavera
{
    public class SessionsHelper
    {
        static string COMPANHIA = InformacaoEmpresa.COMPANHIA;
        static string USER = InformacaoEmpresa.USER;
        static string PASS = InformacaoEmpresa.PASS;

        public static Lib_Primavera.Models.Resposta FazLogin(Models.Session session)
        {
            Lib_Primavera.Models.Resposta resposta = new Models.Resposta();
            
            try
            {
                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {
                    StdBELista objList;

                    Models.Client cli = new Models.Client();
                    string query = "SELECT Cliente FROM  CLIENTES WHERE CDU_Pass = '" + session.Password + 
                        "' AND EnderecoWeb = '" + session.Email + "'";

                    objList = PriEngine.Engine.Consulta(query);

                    while (!objList.NoFim())
                    {
                        cli = new Models.Client();
                        string s = objList.Valor("Cliente");
                        if (s != "VD")
                        {
                            session.CodigoCliente = s;
                            resposta.Codigo = 0;
                            resposta.Descricao = "Sucesso";
                            return resposta;
                        }
                        objList.Seguinte();
                    }
                    resposta.Codigo = 1;
                    resposta.Descricao = "Erro ao encontrar combinacao email/password";
                    return resposta;
                }
                else
                {
                    resposta.Codigo = 1;
                    resposta.Descricao = "Erro ao abrir empresa " + COMPANHIA;
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