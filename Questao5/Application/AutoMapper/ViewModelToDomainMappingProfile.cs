using AutoMapper;
using Questao5.Application.Commands.Requests;
using Questao5.ViewModels;

namespace Questao5.Application.AutoMapper;

public class ViewModelToDomainMappingProfile : Profile
{
    public ViewModelToDomainMappingProfile()
    {
       CreateMap<MovimentoViewModel, MovimentarContaCorrenteCommand>()
            .ConstructUsing(c => new MovimentarContaCorrenteCommand(c.Chave_Idempotencia.HasValue ? c.Chave_Idempotencia.Value : Guid.NewGuid() , c.IdContaCorrente, c.TipoMovimento, c.Valor));
    }
}