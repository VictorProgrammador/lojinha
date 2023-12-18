using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }

        [Column("Color")]
        public string Color { get; set; }
        public string Tamanho { get; set; }
        public Product Product { get; set; }
    }
}
