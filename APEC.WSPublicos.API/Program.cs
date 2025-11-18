using APEC.WS.Aplicacion.Interfaces;
using APEC.WS.Aplicacion.Servicios;
using APEC.WS.Infrastructura.Data;
using APEC.WS.Infrastructura.Modelos;
using APEC.WSPublicos.API;
using APEC.WSPublicos.Aplicacion.Interfaces;
using APEC.WSPublicos.Aplicacion.Servicios;
using Microsoft.EntityFrameworkCore;
using SoapCore;
using SoapCore.Extensibility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDbContextSqlite")));

// Registrar servicios de Application
builder.Services.AddScoped<ITasaCambioService, TasaCambioService>();

// Registrar servicios de Application UDDI
builder.Services.AddScoped<ITasaCambioServiceSoap, TasaCambioServiceSoap>();
builder.Services.AddScoped<IInflacionServiceSoap, InflacionServiceSoap>();
builder.Services.AddScoped<ISaludFinancieraServiceSoap, SaludFinancieraServiceSoap>();
builder.Services.AddScoped<IHistorialCrediticioServiceSoap, HistorialCrediticioServiceSoap>();
builder.Services.AddScoped<IReporteUsoServiceSoap, ReporteUsoServiceSoap>();

// Registrar transformador
builder.Services.AddSingleton<IFaultExceptionTransformer, CustomFaultExceptionTransformer>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetPreflightMaxAge(TimeSpan.FromHours(1));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Middleware de registro de uso
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";

    if (path.StartsWith("/ReporteUso.asmx", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    var serviciosRegistrables = new[]
    {
        "/api",
        "/TasaCambio.asmx",
        "/Inflacion.asmx",
        "/SaludFinanciera.asmx",
        "/HistorialCrediticio.asmx"
    };

    if (serviciosRegistrables.Any(s => path.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
    {
        using var scope = context.RequestServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.RegistrosUso.Add(new RegistroUsoServicio
        {
            NombreServicio = serviciosRegistrables.First(s => path.StartsWith(s, StringComparison.OrdinalIgnoreCase)),
            FechaInvocacion = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }

    await next();
});

// Configuración SOAP
((IApplicationBuilder)app).UseSoapEndpoint<ITasaCambioServiceSoap>("/TasaCambio.asmx", new SoapCore.SoapEncoderOptions(), SoapSerializer.XmlSerializer);
((IApplicationBuilder)app).UseSoapEndpoint<IInflacionServiceSoap>("/Inflacion.asmx", new SoapCore.SoapEncoderOptions(), SoapSerializer.XmlSerializer);
((IApplicationBuilder)app).UseSoapEndpoint<ISaludFinancieraServiceSoap>("/SaludFinanciera.asmx", new SoapCore.SoapEncoderOptions(), SoapSerializer.XmlSerializer);
((IApplicationBuilder)app).UseSoapEndpoint<IHistorialCrediticioServiceSoap>("/HistorialCrediticio.asmx", new SoapCore.SoapEncoderOptions(), SoapSerializer.XmlSerializer);
((IApplicationBuilder)app).UseSoapEndpoint<IReporteUsoServiceSoap>("/ReporteUso.asmx", new SoapCore.SoapEncoderOptions(), SoapSerializer.XmlSerializer);

app.MapControllers();

app.Run();