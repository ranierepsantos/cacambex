using System.Text;
using Domain.Autorizacao;
using Domain.Autorizacao.Interfaces;
using Domain.AzureStorage;
using Domain.Cacambas.Interface;
using Domain.Clientes.Consultas;
using Domain.Clientes.Interface;
using Domain.Identidade.Interfaces;
using Domain.Omie;
using Domain.Omie.Cacambas.Interfaces;
using Domain.Omie.Clientes.Interfaces;
using Domain.Omie.Pedidos.Interface;
using Domain.Pedidos.Consultas;
using Domain.Pedidos.Interface;
using Domain.TipoCacambas.Interface;
using Domain.ViaCep.Interface;
using infra.Omie.Cacambas;
using infra.Omie.Clientes;
using infra.Omie.Pedidos;
using Infra.Dados;
using Infra.Repositorios;
using Infra.Repositorios.Consultas;
using Infra.ViaCep;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace infra;
public static class ConfiguracaoDeServicos
{
    public static IServiceCollection AddInterfacesEServicos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton((Func<IServiceProvider, OmieConfigurations>)(x =>
            {
                string omie_url = configuration.GetSection("OmieConfigurations:OMIE_URL").Value;
                string app_key = configuration.GetSection("OmieConfigurations:APP_KEY").Value;
                string app_secret = configuration.GetSection("OmieConfigurations:APP_SECRET").Value;
                return new(omie_url, app_key, app_secret);
            }));
        services.Configure<StorageContextSettings>(configuration.GetSection(nameof(StorageContextSettings)));
        services.Configure<OmieInformacoesAdicionais>(configuration.GetSection(nameof(OmieInformacoesAdicionais)));
        services.Configure<EmailConfiguracao>(configuration.GetSection(nameof(EmailConfiguracao)));
        services.AddScoped<IAutenticacaoRepositorio, AutenticacaoRepositorio>();
        services.AddScoped<IAutoCadastroRepositorio, AutoCadastroRepositorio>();
        services.AddScoped<IBuscarCep, BuscarCep>();
        services.AddScoped<ICacambaRepositorio, CacambaRepositorio>();
        services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        services.AddScoped<IClienteConsulta, ClienteConsulta>();
        services.AddScoped<IEmailRepositorio, EmailRepositorio>();
        services.AddScoped<ITokenRepositorio, TokenRepositorio>();
        services.AddScoped<IPedidoConsulta, PedidoConsulta>();
        services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
        services.AddScoped<IOmieClientes, OmieClientes>();
        services.AddScoped<IOmieCacambas, OmieCacambas>();
        services.AddScoped<IOmiePedidos, OmiePedidos>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<IFilaSolicitaCacambaRepositorio, FilaSolicitaCacambaRepositorio>();
        services.AddScoped<IFilaRetirarCacambaRepositorio, FilaRetirarCacambaRepositorio>();
        services.AddScoped<IFilaEnviarCacambaRepositorio, FilaEnviarCacambaRepositorio>();
        services.AddScoped<ITipoCacambaRepositorio, TipoCacambaRepositorio>();

        return services;
    }
}
public static class ConfiguracaoBancoDeDados
{
    public static IServiceCollection AddBaseDeDados(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("cacambex")));

        return services;
    }
}
public static class AddServicoAutenticacao
{
    public static IServiceCollection AddServicoDeToken(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenConfiguracao>(configuration.GetSection(nameof(TokenConfiguracao)));
        var key = Encoding.ASCII.GetBytes(configuration.GetSection("TokenConfiguracao:Segredo").Value);
        if (string.IsNullOrEmpty(key.ToString()))
            throw new Exception("TokenConfiguracao:Segredo não encontrado no appsettings.json");
        else
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        return services;
    }
}
public static class AddSwagger
{
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });
        c.AddSecurityRequirement
        (
            new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                }
                }
        );
    });

        return services;
    }
}