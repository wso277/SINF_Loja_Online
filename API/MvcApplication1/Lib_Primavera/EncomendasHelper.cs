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

namespace MvcApplication1.Lib_Primavera
{
    public class EncomendasHelper
    {
        static string COMPANHIA = InformacaoEmpresa.COMPANHIA;
        static string USER = InformacaoEmpresa.USER;
        static string PASS = InformacaoEmpresa.PASS;

        public static Models.Resposta InsereEncomenda(Models.Encomenda dv)
        {
            Lib_Primavera.Models.Resposta resposta = new Models.Resposta();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            //GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            //GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Models.LinhaEncomenda> lstlindv = new List<Models.LinhaEncomenda>();

            try
            {
                if (PriEngine.InitializeCompany(COMPANHIA, USER, PASS) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    myEnc.set_DataDoc(DateTime.Now);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Moeda("EUR");
                    myEnc.set_Serie("A");
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    myEnc.set_CondPag("1");
                    
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasEncomenda;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    
                    foreach (Models.LinhaEncomenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodigoArtigo, lin.Quantidade, "ARM01");
                        PriEngine.Engine.Comercial.ArtigosArmazens.IncrementaQuantidadeReservada(lin.CodigoArtigo, "ARM01", "<L01>", "", lin.Quantidade);
                    }

                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc);
                    PriEngine.Engine.TerminaTransaccao();
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
                PriEngine.Engine.DesfazTransaccao();
                resposta.Codigo = 1;
                resposta.Descricao = ex.Message;
                return resposta;
            }
        }
    }
}