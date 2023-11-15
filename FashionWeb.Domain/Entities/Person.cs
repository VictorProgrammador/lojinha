using System;
namespace FashionWeb.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string Neighborhood { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string WhatsApp { get; set; }
        public string Instagram { get; set; }
        public string PhotoUrl { get; set; }
        public string BannerUrl { get; set; }
        public PersonBusiness PersonBusiness { get; set; }
        public bool HasPhoto { get; set; }
        public bool HasBanner { get; set; }
        public string Cpf { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }
}
