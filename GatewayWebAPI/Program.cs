using GatewayWebAPI.Models;
using GatewayWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using GatewayWebAPI.Controllers;
using GatewayWebAPI.Controller;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var banco = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    banco.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/pagar", PagamentoController.ProcessarPagamento);

app.MapPost("/cadastrar", UsuarioController.CadastrarUsuario);

app.MapPost("/checkout", PagamentoController.ProcessarCheckout);

app.MapGet("/Extrato", UsuarioController.ConsultarExtrato);

app.Run();
