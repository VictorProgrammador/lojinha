using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductTamanho 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TamanhoId { get; set; }
        public Tamanho Tamanho { get; set; }
    }
}
