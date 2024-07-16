using FluentValidation.Results;

namespace Questao5.Application.Queries.Responses;

public class SaldoQueryResponse
{
    public int NumeroContaCorrente { get; set; }
    public string TitularContaCorrente { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public decimal Saldo { get; set; }

    public ValidationResult ValidationResult;

    public SaldoQueryResponse()
    {
        ValidationResult = new ValidationResult();
    }

    public bool HasErrors()
    {
        return !ValidationResult.IsValid;
    }

    public void AddError(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

}
