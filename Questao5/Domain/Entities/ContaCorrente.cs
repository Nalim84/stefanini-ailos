namespace Questao5.Domain.Entities;

public class ContaCorrente : Entity
{
    public string  IdContaCorrente { get; set; }
    public int Numero { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }

    public decimal Saldo(decimal totalDebito, decimal totalCredito)
    {
        return totalCredito - totalDebito;
    }
  
}
