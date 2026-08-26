using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatewayWebAPI.Models
{
    [Table("Vendas")]
    public class VendaCadastro
    {
        public int Id { get; set;  }
        public int ClienteId { get; set; }
        public double Valor { get; set; }
        public DateTime DataHora { get; set;  }

    }
}