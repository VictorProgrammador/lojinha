using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductConfig
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CorId { get; set; }
        public int TamanhoId { get; set; }
        public int Quantidade { get; set; }
        public Cor Cor { get; set; }
        public Tamanho Tamanho { get; set; }
    }
}
