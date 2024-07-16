using System.ComponentModel.DataAnnotations;

namespace Questao5.ViewModels;

public class MovimentoViewModel
{
    public Guid? Chave_Idempotencia { get; set; }
    
    [Required(ErrorMessage = "O id da conta corrente é obrigatório")]
    public Guid IdContaCorrente { get; set; }
    
    [Required(ErrorMessage = "O Tipo de Movimentação é obrigatório.")]
    public string TipoMovimento { get; set; }

    [Required(ErrorMessage = "Valor é obrigatório.")]
    [Range(1, double.MaxValue, ErrorMessage = "Valor deve ser um número positivo.")]

    public double Valor { get; set; }
}
