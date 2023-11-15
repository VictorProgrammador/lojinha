using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public List<CartProduct> CartProducts { get; set; }
    }
}
