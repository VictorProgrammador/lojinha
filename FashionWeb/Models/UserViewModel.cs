using System.ComponentModel.DataAnnotations;

namespace FashionWeb.Models
{
    public class UserViewModel
    {
        [Key]
        public int userid { get; set; }
        public string name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        public string username { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha precisa ter mais que 6 caracteres")]
        public string password { get; set; }
        public int? typeUser { get; set; }
        public string returnUrl { get; set; }
    }
}