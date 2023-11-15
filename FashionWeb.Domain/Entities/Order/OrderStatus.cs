using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities.Order
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public enum OrderStatusType
        {
            Realizado = 1,
            Cancelado = 2
        }

    }
}
