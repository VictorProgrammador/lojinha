using System;

namespace FashionWeb.Domain.Entities
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Guid AspNetUserId { get; set; }
        public Guid UniqueId { get; set; }
        public string Password { get; set; }
        public int typeUser { get; set; }
        public Person Profile { get; set; }
        public string Customer { get; set; }

        public enum Roles
        {
            USUARIO = 0,
            NEGOCIANTE = 1
        }
    }
}
