using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class Raffle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int PersonBusinessId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Image { get; set; }
        public bool IsClosed { get; set; }
        public int QtdNumber { get; set; }
        public Guid UniqueId { get; set; }
    }
}
