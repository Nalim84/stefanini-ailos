using Questao5.Application.Commands.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;

namespace Questao5.Application.Commands.Validations;

public class MovimentoValidation 
{
    public MovimentarContaCorrenteCommand MovimentarContaCorrenteCommand { get; set; }
    public List<string> MensagensErro { get; set; }
    private readonly Dictionary<string, string> ListaMensagensErro;

    public MovimentoValidation(MovimentarContaCorrenteCommand movimentarContaCorrenteCommand, Dictionary<string, string> listaMensagensErro)
    {
        MovimentarContaCorrenteCommand = movimentarContaCorrenteCommand;
        ListaMensagensErro = listaMensagensErro;
    }

    public bool IsValid()
    {
        MensagensErro = new List<string>();

        if (string.IsNullOrEmpty(MovimentarContaCorrenteCommand.TipoMovimento)
            || !ValidarTipoMovimento(MovimentarContaCorrenteCommand.TipoMovimento))
        {
            MensagensErro.Add(ListaMensagensErro[MensagemErroConstants.TipoMovimentoInvalidoKey]);
        }

        if (MovimentarContaCorrenteCommand.Valor <= 0)
        {
            MensagensErro.Add(ListaMensagensErro[MensagemErroConstants.ValorInvalidoKey]);
        }

        return MensagensErro.Count == 0;
    }

    private bool ValidarTipoMovimento(string tipoMovimento)
    {
        char tipoMovChar = char.Parse(tipoMovimento);
        return tipoMovChar == (char)TipoMovimento.Credito || tipoMovChar == (char)TipoMovimento.Debito;
    }
}