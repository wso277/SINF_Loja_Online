﻿using System;
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


        public static Lib_Primavera.Models.Client GetCliente(string codCliente)
        {
            ErpBS objMotor = new ErpBS();

            GcpBECliente objCli = new GcpBECliente();


            Models.Client myCli = new Models.Client();

            if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodigoCliente = objCli.get_Cliente();
                    myCli.Nome = objCli.get_Nome();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Telefone = objCli.get_Telefone();
                    myCli.Email = objCli.get_EnderecoWeb();
                    myCli.Morada = objCli.get_Morada();
                    myCli.Localidade = objCli.get_Localidade();
                    myCli.CodPostal = objCli.get_CodigoPostal();
                    //missing return password because doesn't make any sense
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        private static string getNovoCodigoCliente()
        {
            //ErpBS objMotor = new ErpBS();

            StdBELista objList;

            Models.Client cli = new Models.Client();
            objList = PriEngine.Engine.Consulta("SELECT Cliente FROM  CLIENTES");
            int numClienteMaior = 0;
            while (!objList.NoFim())
            {
                cli = new Models.Client();
                string s = objList.Valor("Cliente");
                if(s != "VD") {
                    s = s.Substring(2, s.Length - 2);
                    int numClienteAtual = Convert.ToInt32(s);
                    if (numClienteAtual > numClienteMaior)
                        numClienteMaior = numClienteAtual;
                    
                }
                objList.Seguinte();
            }
            int numClienteNovo = numClienteMaior + 1;
            string endCodigoCliente = Convert.ToString(numClienteNovo);

            while (endCodigoCliente.Length < 6) {
                endCodigoCliente = "0" + endCodigoCliente;
            }

            string codigoClienteNovo = "CL"+endCodigoCliente;
            return codigoClienteNovo;
        }
        // PUT api/client
        public static Lib_Primavera.Models.Resposta InsereCliente(Models.Client cli)
        {
            Lib_Primavera.Models.Resposta resposta = new Models.Resposta();

            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {
                    string codigoNovoCliente = ClientHelper.getNovoCodigoCliente();
                    myCli.set_Cliente(codigoNovoCliente);
                    myCli.set_Nome(cli.Nome);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda("EUR");

                    myCli.set_EnderecoWeb(cli.Email);


                    myCli.set_Telefone(cli.Telefone);
                    myCli.set_Morada(cli.Morada);
                    myCli.set_Localidade(cli.Localidade);
                    myCli.set_CodigoPostal(cli.CodPostal);
                    //myCli.set_Distrito(cli.CodDistrito);

                    // Inserir campo de utilizador Password
                    StdBECampo campoUtilizadorPassword = new StdBECampo();
                    campoUtilizadorPassword.Nome = "CDU_Pass";
                    campoUtilizadorPassword.Valor = cli.Password;
                    StdBECampos camposUtilizador = new StdBECampos();
                    camposUtilizador.Insere(campoUtilizadorPassword);
                    myCli.set_CamposUtil(camposUtilizador);


                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    resposta.Codigo = 0;
                    resposta.Descricao = "Sucesso";
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

        /*
        // POST api/client
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
        */
    }
}