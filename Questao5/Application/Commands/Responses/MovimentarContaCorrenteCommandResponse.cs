using FluentValidation.Results;

namespace Questao5.Application.Commands.Responses;

public class MovimentarContaCorrenteCommandResponse
{
    public Guid? IdMovimento { get; set; }

    public ValidationResult ValidationResult;

    public MovimentarContaCorrenteCommandResponse()
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
