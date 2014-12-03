using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Lib_Primavera.Models
{
    public class Artigo : ArtigoShort
    {

        /* Especificacoes do utilizador */
        public string Marca { get; set; }
        public string VersaoSistemaOperativo { get; set; } //SubFamilia
        public int RAM { get; set; } //megas - CDU_RAM
        public int Peso { get; set; } //gramas - CDU_PESO
        public int CamaraTraseira { get; set; } //megapixeis - CDU_CAMARA
        public int Armazenamento { get; set; } //gb - CDU_ARMAZENAMENTO
        public float Autonomia { get; set; } //horas - CDU_AUTONOMIA
        public string GPU { get; set; } // CDU_GPU
    }
}