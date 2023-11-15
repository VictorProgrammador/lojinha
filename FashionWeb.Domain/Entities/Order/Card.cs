using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities.Order
{
    public class Card
    {
        public int Id { get; set; }
        public string CpfTitular { get; set; }
        public string Cvv { get; set; }
        public string NomeTitular { get; set; }
        public string NumeroCartao { get; set; }
        public string TelefoneTitular { get; set; }
        public string Validade { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
