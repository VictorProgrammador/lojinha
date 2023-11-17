using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities.Order
{
    public class UserRegister
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public int? typeUser { get; set; }
        public string returnUrl { get; set; }
    }
}
