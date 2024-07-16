using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces;

public interface IIdempotenciaRepository
{
    Task<Idempotencia> ObterIdempotenciaPorId(Guid id);
    Task Adicionar(Idempotencia idempotencia);
}
