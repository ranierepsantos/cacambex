using CTRs.Data;
using CTRs.Repositorios;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(CTRs.Startup))]
namespace CTRs;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddTransient<IClienteRepositorio, ClienteRepositorio>();
        builder.Services.AddOptions<MinhasConfiguracoes>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("MinhasConfiguracoes").Bind(settings);
               });
    }
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);
        FunctionsHostBuilderContext context = builder.GetContext();
        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    }
}
