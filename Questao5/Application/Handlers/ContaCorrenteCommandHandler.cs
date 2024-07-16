using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Application.Commands.Validations;
using Questao5.Domain.Language;
using Questao5.Domain.Interfaces;
using Questao5.Application.Commands.Responses;
using System.Text.Json;

namespace Questao5.Application.Handlers;

public class ContaCorrenteCommandHandler :
    IRequestHandler<MovimentarContaCorrenteCommand, MovimentarContaCorrenteCommandResponse>
{
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;
    private readonly Dictionary<string, string> _listaMensagensErro;

    public ContaCorrenteCommandHandler(
        IIdempotenciaRepository idempotenciaRepository,
        IMovimentoRepository movimentoRepository,
        IContaCorrenteRepository contaCorrenteRepository,
        Dictionary<string, string> listaMensagensErro)
    {
        _idempotenciaRepository = idempotenciaRepository;
        _movimentoRepository = movimentoRepository;
        _contaCorrenteRepository = contaCorrenteRepository;
        _listaMensagensErro = listaMensagensErro;
    }

    public async Task<MovimentarContaCorrenteCommandResponse> Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        var contaCorrente = await _contaCorrenteRepository.ObterPorId(request.IdContaCorrente);
        var response = new MovimentarContaCorrenteCommandResponse();

        if (contaCorrente == null || !contaCorrente.Ativo)
        {
            response.AddError($"{_listaMensagensErro[MensagemErroConstants.ContaInvalidaKey]} ou {_listaMensagensErro[MensagemErroConstants.ContaInativaKey]}");
            return response;
        }

        var movimentoContaCorrenteValidation = new MovimentoValidation(request, _listaMensagensErro);

        if (!movimentoContaCorrenteValidation.IsValid())
        {
            foreach (var mensagemErro in movimentoContaCorrenteValidation.MensagensErro)
            {
                response.AddError(mensagemErro);
            }

            return response;
        }

        if (request.Chave_Idempotencia != Guid.Empty)
        {
            var movimentoExistente = await _idempotenciaRepository.ObterIdempotenciaPorId(request.Chave_Idempotencia);

            if (movimentoExistente != null)
            {
                response.AddError("Movimento já processado anteriormente");
                return response;
            }
        }

        var movimento = new Movimento
        {
            IdMovimento = Guid.NewGuid(),
            IdContaCorrente = request.IdContaCorrente,
            DataMovimento = DateTime.Now.ToString(),
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor
        };

        _movimentoRepository.Adicionar(movimento);
        response.IdMovimento = movimento.IdMovimento;

        await _idempotenciaRepository.Adicionar(new Idempotencia()
        {
            chave_idempotencia = request.Chave_Idempotencia,
            Requisicao = JsonSerializer.Serialize(request),
            Resultado = JsonSerializer.Serialize(response)
        });


        return response;
    }
}