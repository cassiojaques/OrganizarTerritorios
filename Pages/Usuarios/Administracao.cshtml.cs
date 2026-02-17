using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrganizarTerritorios.Helpers;

namespace OrganizarTerritorios.Pages.Usuarios
{
    [Authorize]
    public class AdministracaoUsuariosModel : PageModel
    {
        private readonly Data.AppDbContext _context;
        public List<Models.Usuario> Usuarios { get; set; } = new();

        public AdministracaoUsuariosModel(Data.AppDbContext context)
        {       
            _context = context;
        }

        public void OnGet()
        {
            Usuarios = _context.Usuarios.ToList();
        }

        public IActionResult OnPostAdd([FromForm] Models.Usuario novoUsuario)
        {
            if (!ModelState.IsValid)
            {
                AlertHelper.AlertErro(this, "Erro ao adicionar usuário. Verifique os dados informados.");
                return Page();
            }
            
            // Gerar hash da senha e salvar apenas o hash
            if (!string.IsNullOrWhiteSpace(novoUsuario.SenhaHash))
            {
                novoUsuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novoUsuario.SenhaHash);
            }
            else
            {
                AlertHelper.AlertErro(this, "A senha é obrigatória.");
                return Page();
            }

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
            AlertHelper.AlertSucesso(this, "Usuário adicionado com sucesso!");
            return RedirectToPage();
        }


        public IActionResult OnGetEdit(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return NotFound();

            return Partial("_Editar", usuario);
        }

        public IActionResult OnPostEdit([FromForm] Models.Usuario usuario, [FromForm] string senhaAtual)
        {
            if (!ModelState.IsValid)
            {
                AlertHelper.AlertErro(this, "Erro ao editar usuário. Verifique os dados informados.");
                return Partial("_Editar", usuario);
            }

            var existente = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (existente == null)
            {
                AlertHelper.AlertErro(this, "Usuário não encontrado.");
                return NotFound();
            }

            existente.Email = usuario.Email;   

            // Só permite atualizar a senha se senhaAtual for correta
            if (!string.IsNullOrWhiteSpace(usuario.SenhaHash))
            {
                if (!BCrypt.Net.BCrypt.Verify(senhaAtual, existente.SenhaHash))
                {
                    AlertHelper.AlertErro(this, "A senha atual está incorreta.");
                    return RedirectToPage();
                }
                existente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            }

            _context.SaveChanges();

            AlertHelper.AlertSucesso(this, "Usuário atualizado com sucesso!");
            return RedirectToPage();
        }


        public IActionResult OnGetDelete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return NotFound();

            return Partial("_Deletar", usuario);
        }


        public IActionResult OnPostDelete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                AlertHelper.AlertErro(this, "Usuário não encontrado.");
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            AlertHelper.AlertSucesso(this, "Usuário deletado com sucesso!");
            return RedirectToPage();
        }
    }
}
