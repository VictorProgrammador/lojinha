using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities
{
    public class BinInfo
    {
        public string scheme { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public Country country { get; set; }
        public class Country
        {
            public string name { get; set; }
        }
    }
}
