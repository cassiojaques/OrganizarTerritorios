using System.ComponentModel.DataAnnotations;

namespace OrganizarTerritorios.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string SenhaHash { get; set; } = "";
    }
}
