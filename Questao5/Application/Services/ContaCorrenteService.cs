using AutoMapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.ViewModels;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Services;

public class ContaCorrenteService : IContaCorrenteService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ContaCorrenteService(IMapper mapper, IMediator mediator) { _mediator = mediator; _mapper = mapper; }

    public async Task<MovimentarContaCorrenteCommandResponse> MovimentarContaCorrente(MovimentoViewModel movimentoViewModel)
    {
        var movimentarContaCorrenteCommand = _mapper.Map<MovimentarContaCorrenteCommand>(movimentoViewModel);
        var response = await _mediator.Send<MovimentarContaCorrenteCommandResponse>(movimentarContaCorrenteCommand);
        return response;
    }
    
    public async Task<SaldoQueryResponse> ConsultarSaldo(Guid idContaCorrente)
    {
        var saldoQuery = new SaldoQuery() { IdContaCorrente = idContaCorrente };
        var response = await _mediator.Send(saldoQuery);
        return response;
    }
}
