using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class ProductArchive
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public string UserPath { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
