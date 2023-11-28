using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductType
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
