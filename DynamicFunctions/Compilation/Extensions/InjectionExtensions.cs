using DynamicFunctions.Compilation.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DynamicFunctions.Compilation.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddCompilation(this IServiceCollection services)
    {
        // TryAdd: a consumer compiler registered through the configuration wins.
        services.TryAddSingleton<IFunctionCompiler, CilFunctionCompiler>();

        services
            .AddCompilationStrategy<AddOperatorCompilationStrategy>()
            .AddCompilationStrategy<SubOperatorCompilationStrategy>()
            .AddCompilationStrategy<MultOperatorCompilationStrategy>()
            .AddCompilationStrategy<DivOperatorCompilationStrategy>()
            .AddCompilationStrategy<PowOperatorCompilationStrategy>();

        return services;
    }

    public static IServiceCollection AddCompilationStrategy<T>(this IServiceCollection services)
        where T : class, IOperatorCompilationStrategy
    {
        return services.AddSingleton<IOperatorCompilationStrategy, T>();
    }
}
