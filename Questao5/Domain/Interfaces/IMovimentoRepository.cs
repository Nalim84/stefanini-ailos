using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces;

public interface IMovimentoRepository
{
    Task Adicionar(Movimento movimento);
    Task<decimal> ObterTotalTipoMovimento(Guid IdContaCorrente, string TipoMovimento);
}
