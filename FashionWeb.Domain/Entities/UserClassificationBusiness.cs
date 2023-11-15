using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class UserClassificationBusiness
    {
        public int Id { get; set; }
        public int PersonBusinessId { get; set; }
        public int Rating { get; set; }
        public int PersonId { get; set; }
    }
}
