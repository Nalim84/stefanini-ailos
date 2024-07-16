using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly IDbConnection _dbConnection;

    public ContaCorrenteRepository(IDbConnection dbConnection) {

        _dbConnection = dbConnection;
    }
    
    public async Task<ContaCorrente> ObterPorId(Guid id)
    {
        var conta = await _dbConnection.QuerySingleOrDefaultAsync<ContaCorrente>(
            @"SELECT idcontacorrente, numero, nome, ativo FROM contacorrente WHERE idcontacorrente = @Id AND ativo = 1",
            new { Id = id });

        return conta;
    }
}
