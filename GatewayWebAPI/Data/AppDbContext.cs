using Microsoft.EntityFrameworkCore;
using GatewayWebAPI.Models; 

namespace GatewayWebAPI.Data
{
public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    
public DbSet<Requisicaocadastro> Clientes { get; set; }
public DbSet<VendaCadastro> Vendas { get; set; }

}
}