using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria
{
    internal class ProdutoIndustrializado : Produto
    {
        public string Marca {  get; set; }
        public string CodigoBarras { get; set; }
    }
}
