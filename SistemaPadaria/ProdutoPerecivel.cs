using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria
{
    internal class ProdutoPerecivel : Produto
    {
        public DateTime DataValidade { get; set; }
        public bool Refrigerado { get; set; }
    }
}
