using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductComplement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ComplementId { get; set; }
        public Product Complement { get; set; }
    }
}
