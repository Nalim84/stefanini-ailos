namespace Questao5.Domain.Entities
{
    public class Idempotencia : Entity
    {
        public Guid chave_idempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}
