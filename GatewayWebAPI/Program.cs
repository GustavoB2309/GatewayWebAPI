using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// SIMULAÇÃO DO BANCO DE DADOS (O mesmo dicionário que você dominou na Etapa 2!)
Dictionary<string, double> BancoDeDadosSaldos = new Dictionary<string, double>
{
    { "Jefferson Nogueira", 25.00 },
    { "Edgarth Clinton", 2995.50 }
};

// AQUI ESTÁ A MÁGICA DA ETAPA 3: 
// Criamos uma "Rota" na internet. Quando a Amazon enviar dados para o endereço "/pagar", este bloco roda!
app.MapPost("/pagar", (RequisicaoPagamento dados) =>
{
    // O ASP.NET Core já faz a "Desserialização" do JSON sozinho! Não precisamos do JsonSerializer aqui.

    // 1. Verificamos se o cliente existe no nosso banco de dados
    if (!BancoDeDadosSaldos.ContainsKey(dados.Cliente))
    {
        return Results.BadRequest(new { mensagem = "Erro: Cliente não encontrado no banco de dados." });
    }

    double saldoBanco = BancoDeDadosSaldos[dados.Cliente];

    // 2. Sua lógica de validação perfeita
    if (dados.Cartao && saldoBanco >= dados.Compra)
    {
        // Atualiza o saldo no banco de dados
        BancoDeDadosSaldos[dados.Cliente] = saldoBanco - dados.Compra;
        double novosaldo = BancoDeDadosSaldos[dados.Cliente];

        // Devolvemos um JSON de sucesso de verdade para a internet!
        return Results.Ok(new
        {
            status = "APROVADO",
            cliente = dados.Cliente,
            novoSaldo = novosaldo
        });
    }
    else
    {
        // Devolvemos um JSON de recusa
        return Results.BadRequest(new
        {
            status = "RECUSADO",
            motivo = "Saldo insuficiente ou cartão inválido."
        });
    }
});

app.Run(); 

// O Molde dos dados que vão chegar via internet
class RequisicaoPagamento
{
    public string Cliente { get; set; } = string.Empty;
    public double Compra { get; set; }
    public bool Cartao { get; set; }
}
