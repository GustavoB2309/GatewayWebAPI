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
    }
}