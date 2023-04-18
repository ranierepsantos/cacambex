using FluentValidation.AspNetCore;
using infra;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
{
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
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddInterfacesEServicos(builder.Configuration);
    builder.Services.AddBaseDeDados(builder.Configuration);
    builder.Services.AddServicoDeToken(builder.Configuration);
    builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    app.UseCors();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
