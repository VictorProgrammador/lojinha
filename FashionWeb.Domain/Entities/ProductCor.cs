using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductCor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CorId { get; set; }
        public Cor Cor { get; set; }
    }
}
