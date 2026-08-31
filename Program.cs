using Microsoft.EntityFrameworkCore;
using TaskApi.Infrastructure.Database;
using TaskApi.Features.Tasks;
using MyProject.Api.Features.Tasks;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Em produção o front é servido pelo próprio app (wwwroot), então é mesma origem
// e o CORS nem entra em jogo. Isso aqui existe só para o dev server do Vite.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// cria o arquivo do SQLite e as tabelas na primeira execução (o projeto não usa migrations)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// no Fly o HTTPS é terminado no proxy (force_https no fly.toml), por isso
// o redirect não é feito aqui — evita loop de redirecionamento
app.UseCors("AllowFrontend");

// serve o build do front (frontend/dist copiado para wwwroot no Dockerfile)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapTasksEndpoints();

// qualquer rota que não seja da API cai no index.html do front
app.MapFallbackToFile("index.html");

app.Run();