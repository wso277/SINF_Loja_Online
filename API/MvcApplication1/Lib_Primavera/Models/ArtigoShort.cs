using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class ArtigoShort
    {
        public string CodigoArtigo { get; set; }
        public string Nome { get; set; }
        public float StockAtual { get; set; }


        /** Preço **/
        public float PVP { get; set; }
        public float Desconto { get; set; }

        /* Especificacoes do utilizador */
        public string Marca { get; set; }
        public string NomeSistemaOperativo { get; set; } //Familia
        public float TamanhoEcra { get; set; } //polegadas - CDU_ECRA
        public string CPU { get; set; } // CDU_CPU
        public int Armazenamento { get; set; } //gb - CDU_ARMAZENAMENTO
        public string fotoURL { get; set; } // CDU_FOTO
        public string Lancamento { get; set; } //CDU_Lancamento

        //ordenacao
        public float avaliacao;
    }
}