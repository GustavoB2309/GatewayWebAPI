using Microsoft.EntityFrameworkCore;
using GatewayWebAPI.Models; 

namespace GatewayWebAPI.Data
{
    public class AppDbContext : DbContext
        {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;DataBase=GatewayDB;Trusted_connection=True;TrustServerCertificate=True;");
    }

    public DbSet<Requisicaocadastro> Clientes { get; set; }
}
}