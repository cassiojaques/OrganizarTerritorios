using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrganizarTerritorios.Helpers;

namespace OrganizarTerritorios.Pages.Territorios
{
    [Authorize]
    public class AdministracaoTerritoriosModel : PageModel
    {
        private readonly Data.AppDbContext _context;
        public List<Models.Territorio> Territorios { get; set; } = new();

        public AdministracaoTerritoriosModel(Data.AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Territorios = _context.Territorios.ToList();
        }

        public IActionResult OnPostAdd([FromForm] Models.Territorio novoTerritorio, IFormFile imagem)
        {
            if (!ModelState.IsValid)
            {
                AlertHelper.AlertErro(this, "Erro ao adicionar território. Verifique os dados informados.");
                return Page();
            }

            // Salva imagem se enviada
            if (imagem != null && imagem.Length > 0)
            {
                var folder = Path.Combine("wwwroot", "images", "territorios");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imagem.FileName)}";
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                // Caminho relativo para uso no frontend
                novoTerritorio.UrlImagem = $"/images/territorios/{fileName}";
            }
            _context.Territorios.Add(novoTerritorio);
            _context.SaveChanges();
            AlertHelper.AlertSucesso(this, "Território adicionado com sucesso!");
            return RedirectToPage();
        }

        public IActionResult OnGetEdit(int id)
        {
            var territorio = _context.Territorios.FirstOrDefault(t => t.Id == id);
            if (territorio == null)
                return NotFound();

            return Partial("_Editar", territorio);
        }

        public IActionResult OnPostEdit([FromForm] Models.Territorio territorio, IFormFile imagem)
        {
            if (!ModelState.IsValid)
            {
                AlertHelper.AlertErro(this, "Erro ao editar território. Verifique os dados informados.");
                return Partial("_Editar", territorio);
            }

            var existente = _context.Territorios.FirstOrDefault(t => t.Id == territorio.Id);
            if (existente == null)
            {
                AlertHelper.AlertErro(this, "Território não encontrado.");
                return NotFound();
            }


            existente.Nome = territorio.Nome;
            existente.QuantidadeQuadras = territorio.QuantidadeQuadras;

            // Se um novo arquivo de imagem foi enviado, salva e atualiza o caminho
            if (imagem != null && imagem.Length > 0)
            {
                var folder = Path.Combine("wwwroot", "images", "territorios");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imagem.FileName)}";
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                existente.UrlImagem = $"/images/territorios/{fileName}";
            }

            _context.SaveChanges();

            AlertHelper.AlertSucesso(this, "Território atualizado com sucesso!");
            return RedirectToPage();
        }

        public IActionResult OnGetDelete(int id)
        {
            var territorio = _context.Territorios.FirstOrDefault(t => t.Id == id);
            if (territorio == null) 
                return NotFound();

            return Partial("_Deletar", territorio);
        }

        public IActionResult OnPostDelete(int id)
        {
            var territorio = _context.Territorios.FirstOrDefault(t => t.Id == id);
            if (territorio == null)
            {
                AlertHelper.AlertErro(this, "Território não encontrado.");
                return NotFound();;
            }
            _context.Territorios.Remove(territorio);
            _context.SaveChanges();
            AlertHelper.AlertSucesso(this, "Território deletado com sucesso!");
            return RedirectToPage();
        }
    }
}
