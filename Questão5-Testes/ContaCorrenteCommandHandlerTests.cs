using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;

public class ContaCorrenteCommandHandlerTests
{
    private readonly Mock<IIdempotenciaRepository> _mockIdempotenciaRepository;
    private readonly Mock<IMovimentoRepository> _mockMovimentoRepository;
    private readonly Mock<IContaCorrenteRepository> _mockContaCorrenteRepository;
    private readonly Dictionary<string, string> _mockListaMensagensErro;
    private readonly ContaCorrenteCommandHandler _handler;

    public ContaCorrenteCommandHandlerTests()
    {
        _mockIdempotenciaRepository = new Mock<IIdempotenciaRepository>();
        _mockMovimentoRepository = new Mock<IMovimentoRepository>();
        _mockContaCorrenteRepository = new Mock<IContaCorrenteRepository>();
        _mockListaMensagensErro = new Dictionary<string, string>();

        _handler = new ContaCorrenteCommandHandler(
            _mockIdempotenciaRepository.Object,
            _mockMovimentoRepository.Object,
            _mockContaCorrenteRepository.Object,
            _mockListaMensagensErro
        );
    }

    [Fact]
    public async Task Handle_MovimentoJaProcessado_DeveRetornarErro()
    {
        // Arrange
        var command = new MovimentarContaCorrenteCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "C",
            100
        );

        _mockContaCorrenteRepository.Setup(repo => repo.ObterPorId(It.IsAny<Guid>()))
            .ReturnsAsync(new ContaCorrente { Ativo = true });

        _mockIdempotenciaRepository.Setup(repo => repo.ObterIdempotenciaPorId(It.IsAny<Guid>()))
            .ReturnsAsync(new Idempotencia());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.HasErrors());
        Assert.Contains("Movimento já processado anteriormente", result.ValidationResult.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_MovimentoValido_DeveAdicionarMovimento()
    {
        // Arrange
        var command = new MovimentarContaCorrenteCommand(
            Guid.NewGuid(), // Chave_Idempotencia
            Guid.NewGuid(), // IdContaCorrente
            "C",            // TipoMovimento
            100             // Valor
        );

        _mockContaCorrenteRepository.Setup(repo => repo.ObterPorId(It.IsAny<Guid>()))
            .ReturnsAsync(new ContaCorrente { Ativo = true });

        _mockIdempotenciaRepository.Setup(repo => repo.ObterIdempotenciaPorId(It.IsAny<Guid>()))
            .ReturnsAsync((Idempotencia)null);

        _mockMovimentoRepository.Setup(repo => repo.Adicionar(It.IsAny<Movimento>()))
            .Returns(Task.CompletedTask);

        _mockIdempotenciaRepository.Setup(repo => repo.Adicionar(It.IsAny<Idempotencia>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.HasErrors());
        Assert.NotNull(result.IdMovimento);
        _mockMovimentoRepository.Verify(repo => repo.Adicionar(It.IsAny<Movimento>()), Times.Once);
        _mockIdempotenciaRepository.Verify(repo => repo.Adicionar(It.IsAny<Idempotencia>()), Times.Once);
    }
}