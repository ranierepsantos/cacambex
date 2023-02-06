using System.Text;
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
using Domain.Pedidos.Fila;
using Domain.Pedidos.Interface;
using Domain.ViaCep.Interface;
using FluentValidation.AspNetCore;
using infra.Omie.Cacambas;
using infra.Omie.Clientes;
using infra.Omie.Pedidos;
using Infra.Dados;
using Infra.Pedidos.Fila;
using Infra.Repositorios;
using Infra.Repositorios.Consultas;
using Infra.ViaCep;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
        policy =>
        {
            policy.WithOrigins("http://viacep.com.br/ws")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
builder.Services.AddSingleton((Func<IServiceProvider, StorageContext>)(x =>
{
    string connectionString = builder.Configuration.GetSection("StorageContext:AzureConnString").Value;
    string retirarCacamba = builder.Configuration.GetSection("StorageContext:FilaRetirarCacamba").Value;
    string solicitaCTR = builder.Configuration.GetSection("StorageContext:FilaSolicitaCTR").Value;
    string enviaCacamba = builder.Configuration.GetSection("StorageContext:FilaEnviarCacamba").Value;
    return new(connectionString, retirarCacamba, solicitaCTR, enviaCacamba);
}));
builder.Services.AddSingleton((Func<IServiceProvider, OmieConfigurations>)(x =>
{
    string omie_url = builder.Configuration.GetSection("OmieConfigurations:OMIE_URL").Value;
    string app_key = builder.Configuration.GetSection("OmieConfigurations:APP_KEY").Value;
    string app_secret = builder.Configuration.GetSection("OmieConfigurations:APP_SECRET").Value;
    return new(omie_url, app_key, app_secret);
}));
builder.Services.AddSingleton((Func<IServiceProvider, OmieInformacoesAdicionais>)(x =>
{
    string codigo_categoria = builder.Configuration.GetSection("OmieInformacoesAdicionais:CodigoCategoria").Value;
    long conta_corrente = builder.Configuration.GetSection("OmieInformacoesAdicionais:ContaCorrente").Get<long>();
    return new(conta_corrente, codigo_categoria);
}));
builder.Services.AddSwaggerGen(c =>
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
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAutenticacaoRepositorio, AutenticacaoRepositorio>();
builder.Services.AddScoped<IAutoCadastroRepositorio, AutoCadastroRepositorio>();
builder.Services.AddScoped<IBuscarCep, BuscarCep>();
builder.Services.AddScoped<ICacambaRepositorio, CacambaRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IClienteConsulta, ClienteConsulta>();
builder.Services.AddScoped<IEmailRepositorio, EmailRepositorio>();
builder.Services.AddScoped<IJwtRepositorio, JwtRepositorio>();
builder.Services.AddScoped<IPedidoConsulta, PedidoConsulta>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddScoped<IMensagem, Mensagem>();
builder.Services.AddScoped<IOmieClientes, OmieClientes>();
builder.Services.AddScoped<IOmieCacambas, OmieCacambas>();
builder.Services.AddScoped<IOmiePedidos, OmiePedidos>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IFilaSolicitaCacambaRepositorio, FilaSolicitaCacambaRepositorio>();
builder.Services.AddScoped<IFilaRetirarCacambaRepositorio, FilaRetirarCacambaRepositorio>();
builder.Services.AddScoped<IFilaEnviarCacambaRepositorio, FilaEnviarCacambaRepositorio>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Auth:Secret"]);
builder.Services.AddAuthentication(x =>
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
