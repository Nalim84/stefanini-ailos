using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests;

public class MovimentarContaCorrenteCommand : IRequest<MovimentarContaCorrenteCommandResponse>
{
    public MovimentarContaCorrenteCommand(Guid chave_Idempotencia, Guid idContaCorrente, string tipoMovimento, double valor) {
        Chave_Idempotencia = chave_Idempotencia;
        IdContaCorrente = idContaCorrente;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }

    public Guid Chave_Idempotencia { get; set; }
    public Guid IdContaCorrente { get; set; }
    public string TipoMovimento { get; set; }
    public double Valor { get; set; }
    public bool Ativo { get; set; }
}