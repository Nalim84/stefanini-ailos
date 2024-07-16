using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using Questao5.ViewModels;

namespace Questao5.Application.Services;

public interface IContaCorrenteService
{
    Task<MovimentarContaCorrenteCommandResponse> MovimentarContaCorrente(MovimentoViewModel movimentoViewModel);
    Task<SaldoQueryResponse> ConsultarSaldo(Guid idContaCorrente);
}
