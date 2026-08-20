using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using GatewayWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Dictionary<string, double> BancoDeDadosSaldos = new Dictionary<string, double>
{
    { "Jefferson Nogueira", 25.00 },
    { "Edgarth Clinton", 2995.50 }
};

app.MapPost("/pagar", (RequisicaoPagamento dados) =>
{

    if (string.IsNullOrWhiteSpace(dados.Cliente))
    {
        return Results.BadRequest(new { mensagem = "O nome do cliente não pode estar em branco!" });
    }

    if (!BancoDeDadosSaldos.ContainsKey(dados.Cliente))
        {
            return Results.BadRequest(new { mensagem = "Erro: Cliente não encontrado no banco de dados." });
        }

        double saldoBanco = BancoDeDadosSaldos[dados.Cliente];

        if (dados.Cartao && saldoBanco >= dados.Compra)
        {
            BancoDeDadosSaldos[dados.Cliente] = saldoBanco - dados.Compra;
            double novosaldo = BancoDeDadosSaldos[dados.Cliente];

            return Results.Ok(new
            {
                status = "APROVADO",
                cliente = dados.Cliente,
                novoSaldo = novosaldo
            });
        }
        else
        {
            return Results.BadRequest(new
            {
                status = "RECUSADO",
                motivo = "Saldo insuficiente ou cartão inválido."
            });
        }
});

app.MapPost("/cadastrar", (Requisicaocadastro dados) =>
    {

        if(BancoDeDadosSaldos.ContainsKey(dados.Nome))
        {
            return Results.BadRequest(new { mensagem = "Esse usuário já está cadastrado, tente outro nome!" });
        }
        else
        {
            BancoDeDadosSaldos.Add(dados.Nome, dados.Saldoinicial);
            return Results.Ok(new { mensagem = "Usuário cadastrado com sucesso." });
        }
    });

app.Run();
