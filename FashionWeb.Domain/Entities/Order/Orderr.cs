using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Entities.Order
{
    public class Orderr
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int OrderStatusId { get; set; }
        public string Observation { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public string FreteService { get; set; }
        public int DeliveryRangeMin { get; set; }
        public int DeliveryRangeMax { get; set; }
        public decimal FretePrice { get; set; }
        public int Parcelamento { get; set; }
        public decimal ValorTotal { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string NumeroCasa { get; set; }
        public string Rua { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public int CartId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public UserRegister user { get; set; }
        [Column("Revelado")]
        public bool Revelado { get; set; }
    }
}
