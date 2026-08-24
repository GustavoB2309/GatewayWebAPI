using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using GatewayWebAPI.Models;
using GatewayWebAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// === PASSO 1: CRIAÇÃO DO BANCO SE ELE NÃO EXISTIR (LOGA DIRETAMENTE NO APP PRONTO) ===
using (var banco = new AppDbContext())
{
    banco.Database.EnsureCreated();
}

// SIMULAÇÃO DO BANCO ANTIGO EM MEMÓRIA (Mantido para a rota /pagar não quebrar)
Dictionary<string, double> BancoDeDadosSaldos = new Dictionary<string, double>
{
    { "Jefferson Nogueira", 25.00 },
    { "Edgarth Clinton", 2995.50 }
};

// === ROTA 1: PROCESSAMENTO DO PAGAMENTO (Usando o Dicionário temporariamente) ===
app.MapPost("/pagar", (RequisicaoPagamento dados) =>
{
    using var banco = new AppDbContext();

    if (string.IsNullOrWhiteSpace(dados.Cliente))
    {
        return Results.BadRequest(new { mensagem = "O nome do cliente não pode estar em branco!" });
    }

     var clienteNoBanco = banco.Clientes.FirstOrDefault(c => c.Nome == dados.Cliente);

    if (clienteNoBanco == null)
    {
        return Results.BadRequest(new { mensagem = "Erro: Cliente não encontrado no banco de dados." });
    }
    else if (dados.Compra > 0)
    {
        clienteNoBanco.Saldoinicial = clienteNoBanco.Saldoinicial - dados.Compra;
        banco.SaveChanges();
        return Results.Ok(new { mensagem = "Compra bem sucedida!" });
    }
    else
    {
        return Results.BadRequest(new { mensagem = "O valor da compra não pode ser zero ou inferior" });
    }

});

// === ROTA 2: CADASTRO REAL NO BANCO DE DADOS (GRAVANDO NO HD!) ===
app.MapPost("/cadastrar", (Requisicaocadastro dados) =>
{

    using var banco = new AppDbContext();

    // Procura se o nome já existe na tabela do SQL Server
    var usuarioExiste = banco.Clientes.Any(c => c.Nome == dados.Nome);

    if (usuarioExiste)
    {
        return Results.BadRequest(new { mensagem = "Esse usuário já está cadastrado no HD, tente outro nome!" });
    }
    else
    {
        // Adiciona e salva fisicamente no banco de dados real
        banco.Clientes.Add(dados);
        banco.SaveChanges();

        return Results.Ok(new { mensagem = "Usuário cadastrado com sucesso no BANCO DE DADOS!" });
    }
});

app.MapPost("/checkout", (RequisicaoPagamento dados) =>
{
    using var banco = new AppDbContext();

    Console.WriteLine($"[{DateTime.Now}] INFO: Iniciando Checkout de {dados.Cliente}");

    if (string.IsNullOrWhiteSpace(dados.Cliente))
    {
        return Results.BadRequest(new { mensagem = "O nome do cliente não pode vir em branco" });
    }

    var clientenobanco = banco.Clientes.FirstOrDefault(c => c.Nome == dados.Cliente);

    if (clientenobanco == null)
    {
        var novoCliente = new Requisicaocadastro
        {
            Nome = dados.Cliente,
            Saldoinicial = 500
        };

        banco.Clientes.Add(novoCliente);
        banco.SaveChanges();

        clientenobanco = novoCliente;
    }

    Console.WriteLine($"[{DateTime.Now}] INFO: Iniciando processo de compra para {dados.Cliente}");

if (dados.Compra <= 0 || clientenobanco.Saldoinicial < dados.Compra)
    {
        return Results.BadRequest(new { mensagem = "O valor da compra não pode ser 0 ou menor ou não há saldo suficiente." });
    }

    clientenobanco.Saldoinicial = clientenobanco.Saldoinicial - dados.Compra;
    banco.SaveChanges();

    Console.WriteLine($"[{DateTime.Now}] INFO: Checkout concluído com sucesso (pago) para {dados.Cliente}");

    return Results.Ok(new { status = "CONCLUÍDO!", mensagem = "Checkout completo.", novosaldo = clientenobanco.Saldoinicial });

});

// === PASSO FINAL: ISSO FAZ O SERVIDOR FICAR LIGADO SINALIZANDO A REDE! ===
app.Run();
