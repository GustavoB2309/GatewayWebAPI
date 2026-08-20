namespace GatewayWebAPI.Models
{
    class RequisicaoPagamento
    {
        public string Cliente { get; set; } = string.Empty;
        public double Compra { get; set; }
        public bool Cartao { get; set; }
    }

    class Requisicaocadastro
    {
        public string Nome { get; set; } = string.Empty;
        public double Saldoinicial { get; set; }
    }
}
