using ListaDeContatos.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<ContatoContext>(o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("Database"),
        options => options.EnableRetryOnFailure()));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Contatos}/{action=Index}/{id?}");

try
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var db = serviceProvider.GetRequiredService<ContatoContext>();
        db.Database.Migrate();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao criar/migrar o banco de dados: {ex.Message}");
}


app.Run();
