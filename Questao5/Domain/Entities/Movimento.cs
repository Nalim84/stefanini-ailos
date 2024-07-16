namespace Questao5.Domain.Entities;

public class Movimento : Entity
{
    public Guid IdMovimento { get; set; }
    public Guid IdContaCorrente { get; set; }
    public string DataMovimento { get; set; }
    public string TipoMovimento { get; set; }
    public double Valor { get; set; }
}
