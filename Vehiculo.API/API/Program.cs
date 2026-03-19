using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using Autorizacion.Middleware;
using DA;
using DA.Repositorio;
using Flujo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Reglas;
using Servicios;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Vehiculo API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT de esta forma: Bearer {tu_token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });

});



var tokenConfig = builder.Configuration.GetSection("Token").Get<TokenConfiguracion>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(builder.Configuration["Token:key"]))
        };


    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            // Esto envía el error al "Response Header" que verás en Swagger
            context.Response.OnStarting(async () =>
            {
                context.Response.Headers.Add("WWW-Authenticate", $"Bearer error=\"{context.Exception.Message}\"");
            });
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // Si entra aquí es porque ni siquiera se envió el token
            if (string.IsNullOrEmpty(context.Error))
            {
                context.Response.Headers.Add("X-Error-Debug", "No se recibio ningun token en el Header");
            }
            return Task.CompletedTask;
        }
    };
    });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IVehiculoFlujo, VehiculoFLujo>();
builder.Services.AddScoped<IVehiculoDA, VehiculoDA>();
builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();
builder.Services.AddScoped<IRegistroServicio, RegistroServicio>();
builder.Services.AddScoped<IRevisionServicio, RevisionServicio>();
builder.Services.AddScoped<IConfiguracion, Configuracion>();
builder.Services.AddScoped<IRevisionReglas, RevisionReglas>();
builder.Services.AddScoped<IRegistroReglas,RegistroReglas>();
builder.Services.AddScoped<IMarcaDA, MarcaDA>();
builder.Services.AddScoped<IModeloDA, ModeloDA>();
builder.Services.AddScoped<IMarcaFlujo, MarcaFlujo>();
builder.Services.AddScoped<IModeloFlujo, ModeloFlujo>();


builder.Services.AddTransient<Autorizacion.Abstracciones.Flujo.IAutorizacionFlujo,
                               Autorizacion.Flujo.AutorizacionFlujo>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.ISeguridadDA,
                               Autorizacion.DA.SeguridadDA>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.IRepositorioDapper,
                               Autorizacion.DA.Repositorios.RepositorioDapper>();



var politicaAcceso = "Politica de acceso";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: politicaAcceso,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost", "https://localhost:50427", "https://localhost:50428","https://localhost:7032")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
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

app.UseCors(politicaAcceso);

app.UseAuthentication();
app.AutorizacionClaims();
app.UseAuthorization();    


app.MapControllers();

app.Run();
