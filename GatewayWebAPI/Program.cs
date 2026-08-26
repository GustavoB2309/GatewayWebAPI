using GatewayWebAPI.Models;
using GatewayWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using GatewayWebAPI.Controllers;

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

app.MapPost("/cadastrar", (Requisicaocadastro dados, AppDbContext banco) =>
{

    var usuarioExiste = banco.Clientes.Any(c => c.Nome == dados.Nome);

    if (usuarioExiste)
    {
        return Results.BadRequest(new { mensagem = "Esse usuário já está cadastrado no HD, tente outro nome!" });
    }
    else
    {
        banco.Clientes.Add(dados);
        banco.SaveChanges();

        return Results.Ok(new { mensagem = "Usuário cadastrado com sucesso no BANCO DE DADOS!" });
    }
});

app.MapPost("/checkout", PagamentoController.ProcessarCheckout); 

app.Run();
