using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class CheckinPersonBusinessRaffle
    {
        public int Id { get; set; }
        public int PersonBusinessId { get; set; }
        public string Instagram { get; set; }
        public int RaffleId { get; set; }
        public DateTime CreateDate { get; set; }
        public int SortedNumber { get; set; }
    }
}
