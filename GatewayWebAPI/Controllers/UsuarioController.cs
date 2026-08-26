using Microsoft.AspNetCore.Http;
using GatewayWebAPI.Models;
using GatewayWebAPI.Data;

namespace GatewayWebAPI.Controller
{
    public static class UsuarioController
    {
        public static IResult CadastrarUsuario(Requisicaocadastro dados, AppDbContext banco)
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
        }

        public static IResult ConsultarExtrato(string nomeCliente, AppDbContext banco)
        {
            Console.WriteLine($"[{DateTime.Now}] INFO: Consulta de extrato solitado por '{nomeCliente}'");

            if (string.IsNullOrWhiteSpace(nomeCliente))
            {
                Console.WriteLine($"[{DateTime.Now}] ERRO: Nome vazio '{nomeCliente}'");
                return Results.BadRequest(new { mensagem = "O nome do cliente não pode estar vazio." });
            }

            try
            {
                var cliente = banco.Clientes.FirstOrDefault(c => c.Nome == nomeCliente);

                if (cliente == null)
                {
                    Console.WriteLine($"[{DateTime.Now}] ERRO: Cliente não encontrado no banco de dados para extrato '{nomeCliente}'");
                    return Results.BadRequest(new { mensagem = "Erro: Cliente não encontrado no banco de dados." });
                }

                var historicoDeVendas = banco.Vendas
                    .Where(v => v.ClienteId == cliente.Id)
                    .OrderByDescending(v => v.DataHora)
                    .ToList();

                Console.WriteLine($"[{DateTime.Now}] INFO: Consulta de extrato concluída por '{nomeCliente}'");
                return Results.Ok(new { cliente = cliente.Nome, saldoAtual = cliente.Saldoinicial, extrato = historicoDeVendas });
            }
            catch (Exception erro)
            {
                Console.WriteLine($"[{DateTime.Now}] CRÍTICO: Falha ao ler extrato no SQL. Detalhes: '{erro.Message}'");
                return Results.Problem("Erro interno ao processar extrato bancário.");
            }

            }
    }

}