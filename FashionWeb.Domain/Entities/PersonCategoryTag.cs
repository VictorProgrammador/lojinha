using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class PersonCategoryTag
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int CategoryId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
