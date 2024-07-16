using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests;

public class SaldoQuery : IRequest<SaldoQueryResponse>
{
    public Guid IdContaCorrente { get; set; }
}