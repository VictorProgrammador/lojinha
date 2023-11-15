using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure.Response
{
    public class ResponseSaveCheckinPersonBusiness
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Dictionary<string, int> PedrasSorteadas { get; set; }
    }
}
