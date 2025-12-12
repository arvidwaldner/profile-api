using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ProfileApi.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProfileDataService, ProfileDataService>();
builder.Services.AddScoped<ISkillSortingService, SkillSortingService>();
builder.Services.AddScoped<IWorkExperiencesSortingService, WorkExperiencesSortingService>();
builder.Services.AddScoped<ICertificationsSortingService, CertificationsSortingService>();
builder.Services.AddScoped<IEducationsSortingService, EducationsSortingService>();
builder.Services.AddScoped<ILanguageSkillsSortingService, LanguageSkillsSortingService>();
builder.Services.AddScoped<ISkillAreasSortingService, SkillAreasSortingService>();

var signingKey = builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");

Console.WriteLine("Jwt__Audiences env: " + Environment.GetEnvironmentVariable("Jwt__Audiences"));
var audiences = builder.Configuration.GetSection("Jwt:Audiences").Get<string[]>();
Console.WriteLine("Loaded audiences: " + string.Join(", ", audiences ?? Array.Empty<string>()));

//audiences = builder.Configuration.GetSection("Jwt:Audiences").Get<string[]>()
//    ?? throw new InvalidOperationException("JWT audiences are not configured.");

var signingKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudiences = audiences,
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
            "http://localhost:4200",      // Angular default dev port
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

// Log all endpoints
Console.WriteLine("Mapped endpoints:");
app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
    foreach (var endpoint in endpointDataSource.Endpoints)
    {
        var routeEndpoint = endpoint as RouteEndpoint;
        if (routeEndpoint != null)
        {
            logger.LogInformation("Mapped route: {RoutePattern} ({DisplayName})", routeEndpoint.RoutePattern.RawText, endpoint.DisplayName);
        }
        else
        {
            logger.LogInformation("Mapped endpoint: {DisplayName}", endpoint.DisplayName);
        }
    }
});
Console.WriteLine("Mapped endpoints DONE:");
app.Run();
