using Moq;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Interfaces;
using Questao5.Domain.Entities;

public class SaldoQueryHandlerTests
{
    private readonly Mock<IMovimentoRepository> _mockMovimentoRepository;
    private readonly Mock<IContaCorrenteRepository> _mockContaCorrenteRepository;
    private readonly Dictionary<string, string> _mockListaMensagensErro;
    private readonly SaldoQueryHandler _handler;

    public SaldoQueryHandlerTests()
    {
        _mockMovimentoRepository = new Mock<IMovimentoRepository>();
        _mockContaCorrenteRepository = new Mock<IContaCorrenteRepository>();
        _mockListaMensagensErro = new Dictionary<string, string>();
           
        _handler = new SaldoQueryHandler(
            _mockMovimentoRepository.Object,
            _mockContaCorrenteRepository.Object,
            _mockListaMensagensErro
        );
    }

    [Fact]
    public async Task Handle_ContaCorrenteValida_DeveRetornarSaldo()
    {
        // Arrange
        var contaCorrente = new ContaCorrente
        {
            Ativo = true,
            Numero = 12345,
            Nome = "Titular da Conta"
        };

        _mockContaCorrenteRepository.Setup(repo => repo.ObterPorId(It.IsAny<Guid>()))
            .ReturnsAsync(contaCorrente);

        _mockMovimentoRepository.Setup(repo => repo.ObterTotalTipoMovimento(It.IsAny<Guid>(), "C"))
            .ReturnsAsync(1000);
        _mockMovimentoRepository.Setup(repo => repo.ObterTotalTipoMovimento(It.IsAny<Guid>(), "D"))
            .ReturnsAsync(500);

        var request = new SaldoQuery { IdContaCorrente = Guid.NewGuid() };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.HasErrors());
        Assert.Equal(contaCorrente.Numero, result.NumeroContaCorrente);
        Assert.Equal(contaCorrente.Nome, result.TitularContaCorrente);
        Assert.Equal(500, result.Saldo);
        Assert.Equal(DateTime.Now.Date, result.DataHoraConsulta.Date);
    }
}