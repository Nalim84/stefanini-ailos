namespace Questao5.Domain.Language;

public static class MensagemErroConstants
{
    public static Dictionary<string, string> Mensagens { get; set; }

    public  const string ContaInvalidaKey = "INVALID_ACCOUNT";
    public  const string ContaInativaKey = "INACTIVE_ACCOUNT";
    public  const string ValorInvalidoKey = "INVALID_VALUE";
    public  const string TipoMovimentoInvalidoKey = "INVALID_TYPE";
}
