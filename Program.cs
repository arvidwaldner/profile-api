using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ProfileApi.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfileApi.Configurations;
using Microsoft.Extensions.Configuration;
using ProfileApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProfileDataService, ProfileDataService>();
builder.Services.AddScoped<ISkillSortingService, SkillSortingService>();
builder.Services.AddScoped<IWorkExperiencesSortingService, WorkExperiencesSortingService>();
builder.Services.AddScoped<ICertificationsSortingService, CertificationsSortingService>();
builder.Services.AddScoped<IEducationsSortingService, EducationsSortingService>();
builder.Services.AddScoped<ILanguageSkillsSortingService, LanguageSkillsSortingService>();
builder.Services.AddScoped<ISkillAreasSortingService, SkillAreasSortingService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

// Bind configuration to ConfigurationSetup instance
var configSetup = builder.Configuration.Get<ConfigurationSetup>() ?? new ConfigurationSetup();

// Register as singleton for direct injection
builder.Services.AddSingleton<IConfigurationSetup>(configSetup);

ValidateConfiguration(configSetup);
void ValidateConfiguration(ConfigurationSetup config)
{
    if (config.Jwt == null)
        throw new InvalidOperationException("Jwt configuration section is missing.");

    if (string.IsNullOrWhiteSpace(config.Jwt.Issuer) || string.Equals(config.Jwt.Issuer, "your-issuer-here", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("Jwt.Issuer is not set or is a placeholder.");

    if (string.IsNullOrWhiteSpace(config.Jwt.SigningKey) || string.Equals(config.Jwt.SigningKey == "your-signing-key-here", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("Jwt.SigningKey is not set or is a placeholder.");

    if (config.Jwt.Audiences == null || !config.Jwt.Audiences.Any()) 
        throw new InvalidOperationException("Jwt.Audiences are missing");

    if (config.ApiKeys == null || !config.ApiKeys.Any() || config.ApiKeys.Values.Any(v => string.IsNullOrWhiteSpace(v) || v == "your-service-api-key-here"))
        throw new InvalidOperationException("ApiKeys are not set or contain a placeholder.");

    if(config.AudienceApiKeyMap == null || !config.AudienceApiKeyMap.Any())
        throw new InvalidOperationException("AudienceApiKeyMap is missing or empty.");    
}

var signingKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configSetup.Jwt.SigningKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configSetup.Jwt.Issuer,
        ValidAudiences = configSetup.Jwt.Audiences,
        IssuerSigningKey = signingKeyBytes,
    };    
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Profile API",
        Version = "v1",
        Description = "REST API serving portfolio data"
    });

    // Add JWT Bearer security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add security requirement for all endpoints
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
            Array.Empty<string>()
        }
    });    
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    // Fixed window policy: 100 requests per minute per IP
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(
            "https://arvidwaldner.github.io/profile-angular-app",            
            "https://localhost:4200"     // HTTPS variant            
        )
        .WithMethods("GET", "POST")               
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase        
    });

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowedOrigins");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
