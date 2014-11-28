using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class Artigo
    {
        public string CodigoArtigo { get; set; }
        public string Nome { get; set; }
        public float StockAtual { get; set; }


        /** Preço **/
        public float PVP { get; set; }
        public float Desconto { get; set; }

        /* Especificacoes do joao */
        public string Marca { get; set; }
        public string NomeSistemaOperativo { get; set; } //Familia
        public string VersaoSistemaOperativo { get; set; } //SubFamilia
        public float TamanhoEcra { get; set; } //polegadas
        public int RAM { get; set; } //megas
        public string CPU { get; set; }
        public int Peso { get; set; } //gramas
        public int CamaraTraseira { get; set; } //megapixeis
        public int Armazenamento { get; set; } //gb
        public float Autonomia { get; set; } //horas
        public string GPU { get; set; }
    }
}