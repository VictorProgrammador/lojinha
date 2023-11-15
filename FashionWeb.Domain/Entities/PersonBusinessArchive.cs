using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class PersonBusinessArchive
    {
        public int Id { get; set; }
        public int PersonBusinessId { get; set; }
        public string UserPath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
    }
}
