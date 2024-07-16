using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly IDbConnection _dbConnection;

    public MovimentoRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task Adicionar(Movimento movimento)
    {
        await _dbConnection.ExecuteAsync(
@"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
            movimento);
    }

    public async Task<decimal> ObterTotalTipoMovimento(Guid IdContaCorrente, string TipoMovimento)
    {
        var valor = await _dbConnection.QuerySingleAsync<decimal>(
        "SELECT COALESCE(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = @TipoMovimento",
        new { IdContaCorrente, TipoMovimento });

        return valor;
    }
}
