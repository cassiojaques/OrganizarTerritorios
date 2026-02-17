using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrganizarTerritorios.DTOs;

namespace OrganizarTerritorios.Pages.Territorios
{
    [Authorize]
    public class TerritoriosModel : PageModel
    {
        private readonly Data.AppDbContext _context;
        public TerritoriosModel(Data.AppDbContext context)
        {
            _context = context;
        }

        public List<Models.Territorio> Territorios { get; set; } = new();

        public void OnGet()
        {
            Territorios = _context.Territorios.ToList();
        }
        
        public IActionResult OnGetDetalhesPartial(int id)
        {
            var territorio = _context.Territorios.FirstOrDefault(t => t.Id == id);
            if (territorio == null) return NotFound();
            return Partial("_Detalhes", territorio);
        }

        public IActionResult OnPostAtualizarQuadras([FromBody] AtualizarQuadrasDTO request)
        {
            if (request == null)
                return BadRequest("Body vazio");

            var territorio = _context.Territorios
                .FirstOrDefault(t => t.Id == request.TerritorioId);

            if (territorio == null)
                return NotFound();

            territorio.QuadrasFeitas = string.Join(",", request.QuadrasFeitas ?? new List<int>());
            _context.SaveChanges();

            return new JsonResult(new { success = true });
        }

    }
}
