using System.ComponentModel.DataAnnotations;

namespace OrganizarTerritorios.Models
{
    public class Territorio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? UrlImagem { get; set; }
        public int QuantidadeQuadras { get; set; }
        public string? QuadrasFeitas { get; set; }
    }
}
