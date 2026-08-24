namespace GatewayWebAPI.Models
{
    public class RequisicaoPagamento
    {
        public string Cliente { get; set; } = string.Empty;
        public double Compra { get; set; }
        public bool Cartao { get; set; }
    }

    public class Requisicaocadastro
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double Saldoinicial { get; set; }
    }
}
