using System;
using System.Collections.Generic;
using System.Text;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public struct CategoriaFiltro
    {
        public int CodigoCategoria { get; set; }
        public string Categoria { get; set; }

        public CategoriaFiltro(int codigoCategoria, string categoria) : this()
        {
            CodigoCategoria = codigoCategoria;
            Categoria = categoria;
        }
    }
}
