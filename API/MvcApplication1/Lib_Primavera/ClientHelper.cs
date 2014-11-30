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
    public class ClientHelper
    {
        static string COMPANHIA = InformacaoEmpresa.COMPANHIA;
        static string USER = InformacaoEmpresa.USER;
        static string PASS = InformacaoEmpresa.PASS;

        public static Lib_Primavera.Models.Resposta InsereCliente(Models.Client cli)
        {
            Lib_Primavera.Models.Resposta resposta = new Models.Resposta();

            GcpBECliente myCli = new GcpBECliente();
            
            try{
                if( PriEngine.InitializeCompany(COMPANHIA, USER, PASS ) == true ){

                    myCli.set_Cliente(cli.CodigoCliente);
                    myCli.set_Nome(cli.Nome);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda("EUR");
                    
                    myCli.set_EnderecoWeb(cli.Email);


                    myCli.set_Telefone(cli.Telefone);
                    myCli.set_Morada(cli.Morada);
                    myCli.set_Localidade(cli.Localidade);
                    myCli.set_CodigoPostal(cli.CodPostal);
                    myCli.set_Distrito(cli.CodDistrito);        
                    
                    // Inserir campo de utilizador Password
                    StdBECampo campoUtilizadorPassword = new StdBECampo();
                    campoUtilizadorPassword.Nome = "CDU_CampoVar1";
                    campoUtilizadorPassword.Valor = cli.Password;
                    StdBECampos camposUtilizador = new StdBECampos();
                    camposUtilizador.Insere(campoUtilizadorPassword);
                    myCli.set_CamposUtil(camposUtilizador);
                    
                    
                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    resposta.Codigo = 0;
                    resposta.Descricao = "Sucesso";
                    return resposta;
                }
                else {
                    resposta.Codigo = 1;
                    resposta.Descricao = "Erro ao abrir empresa " + COMPANHIA;
                    return resposta;
                }
            }

            catch(Exception ex) {
                resposta.Codigo = 1;
                resposta.Descricao = ex.Message;
                return resposta;
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
                        if( cliente.Nome != "" )
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
                    resposta.Descricao = "Erro ao abrir a empresa "+ COMPANHIA;
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