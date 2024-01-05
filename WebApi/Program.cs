using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Applications;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces;
using SampleDotnetCleanArchitecture.Domain.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Services;
using SampleDotnetCleanArchitecture.Infrastructure.Security;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration["DATABASE_CONNECTION_STRING"];

builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddScoped<IAccountApplication, AccountApplication>();
builder.Services.AddScoped<IClientApplication, ClientApplication>();


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IDbConnectionProject>(sp => new DbConnectionProject(connectionString));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountSecurity, AccountSecurity>();

builder.Services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    var key = Encoding.ASCII.GetBytes("minha_chave_secreta_muito_segura");
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    foreach (var name in Enum.GetNames<AccountClaims>())
    {
        options.AddPolicy(name, policy => policy.RequireClaim(name));
    }
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configurar a segurança do Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inset the JWT Token with Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();