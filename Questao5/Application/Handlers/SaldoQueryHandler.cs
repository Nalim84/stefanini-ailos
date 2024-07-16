using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces;
using Questao5.Domain.Language;


namespace Questao5.Application.Handlers;

public class SaldoQueryHandler
    : IRequestHandler<SaldoQuery, SaldoQueryResponse>
{
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly Dictionary<string, string> _listaMensagensErro;


    public SaldoQueryHandler(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository, Dictionary<string, string> listaMensagensErro)
    {
        _movimentoRepository = movimentoRepository;
        _contaCorrenteRepository = contaCorrenteRepository;
        _listaMensagensErro = listaMensagensErro;
    }

    public async Task<SaldoQueryResponse> Handle(SaldoQuery request, CancellationToken cancellationToken)
    {
        var contaCorrente = await _contaCorrenteRepository.ObterPorId(request.IdContaCorrente);
        var response = new SaldoQueryResponse();

        if (contaCorrente == null || !contaCorrente.Ativo)
        {
            response.AddError($"{_listaMensagensErro[MensagemErroConstants.ContaInvalidaKey]} ou {_listaMensagensErro[MensagemErroConstants.ContaInativaKey]}");
            return response;
        }

        var credito = await _movimentoRepository.ObterTotalTipoMovimento(request.IdContaCorrente, "C");
        var debito = await _movimentoRepository.ObterTotalTipoMovimento(request.IdContaCorrente, "D");

        response.NumeroContaCorrente = contaCorrente.Numero;
        response.TitularContaCorrente = contaCorrente.Nome;
        response.DataHoraConsulta = DateTime.Now;
        response.Saldo = contaCorrente.Saldo(debito, credito);

        return response;
    }
}
