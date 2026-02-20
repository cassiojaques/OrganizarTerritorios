using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OrganizarTerritorios.Data;
using OrganizarTerritorios.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
    });
builder.Services.AddAuthorization();

// Configura a porta para o ambiente de produção do Render
//var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
//builder.WebHost.UseUrls($"http://*:{port}");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

SeedDatabase(app);

app.Run();

void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate(); // garante que o banco existe

    if (!db.Usuarios.Any())
    {
        var usuario = new Usuario
        {
            Email = "cassio.jaques@gmail.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("vendas123")
        };

        db.Usuarios.Add(usuario);
        db.SaveChanges();
    }
}