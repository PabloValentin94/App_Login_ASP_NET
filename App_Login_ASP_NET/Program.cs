using App_Login_ASP_NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using App_Login_ASP_NET.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PseudoDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PseudoDatabaseContext") ?? throw new InvalidOperationException("Connection string 'PseudoDatabaseContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Definindo os valores dos atributos de configura��o do arquivo de contexto do MongoDB.

MongoDBContext.Connection_String = builder.Configuration.GetSection("MongoDBConnection:ConnectionString").Value;

MongoDBContext.Database_Name = builder.Configuration.GetSection("MongoDBConnection:DatabaseName").Value;

MongoDBContext.Is_Ssl = Convert.ToBoolean(builder.Configuration.GetSection("MongoDBConnection:IsSsl").Value);

// Configurando os recursos de autentica��o (Login).

builder.Services.AddIdentity<AppUser, AppRole>().AddMongoDbStores<AppUser, AppRole, Guid>(MongoDBContext.Connection_String, MongoDBContext.Database_Name);

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
