using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class Charge
    {
        public string Cartao { get; set; }
        public bool Paid { get; set; }
        public string FailureMessage { get; set; }
        public DateTime Created { get; set; }
    }
}
