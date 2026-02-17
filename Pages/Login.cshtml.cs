using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrganizarTerritorios.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace OrganizarTerritorios.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;

        public LoginModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Senha { get; set; } = "";

        public string? Erro { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == Email);

            if (usuario == null ||
                !BCrypt.Net.BCrypt.Verify(Senha, usuario.SenhaHash))
            {
                Erro = "Email ou senha inválidos";
                return Page();
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email)
        };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToPage("/Index");
        }
    }
}
