using AutoMapper;
using DevMS.App.Extensions;
using DevMS.Business.Interfaces;
using DevMS.Data.Context;
using DevMS.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace DevMS.App.Configurations
{

    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ResolveConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<MeuDbContext>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdpterProvider>();

            return services;
        }
    }
}
