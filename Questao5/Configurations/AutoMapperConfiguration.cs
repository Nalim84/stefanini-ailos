using Questao5.Application.AutoMapper;

namespace Questao5.Configurations;

public static class AutoMapperConfiguration
{
    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
    }
}
