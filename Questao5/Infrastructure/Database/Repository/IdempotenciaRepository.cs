using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly IDbConnection _dbConnection;

    public IdempotenciaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Idempotencia> ObterIdempotenciaPorId(Guid chave_idempotencia)
    {
        var idempotencia = await _dbConnection.QuerySingleOrDefaultAsync<Idempotencia>(
        @"SELECT requisicao, resultado  FROM idempotencia WHERE chave_idempotencia = @chave_idempotencia",
        new { chave_idempotencia = chave_idempotencia });

        return idempotencia;

    }

    public async Task Adicionar(Idempotencia idempotencia)
    {
        await _dbConnection.ExecuteAsync(
            "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@chave_idempotencia, @Requisicao, @Resultado)",
                idempotencia);
    }
}