using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ProfileApi.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProfileDataService, ProfileDataService>();
builder.Services.AddScoped<ISkillSortingService, SkillSortingService>();
builder.Services.AddScoped<IWorkExperiencesSortingService, WorkExperiencesSortingService>();
builder.Services.AddScoped<ICertificationsSortingService, CertificationsSortingService>();
builder.Services.AddScoped<IEducationsSortingService, EducationsSortingService>();
builder.Services.AddScoped<ILanguageSkillsSortingService, LanguageSkillsSortingService>();
builder.Services.AddScoped<ISkillAreasSortingService, SkillAreasSortingService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer",
            ValidAudience = "yourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSuperSecretKey"))
        };
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
            "https://arvidwaldner.github.io",
            "http://localhost:4200",      // Angular default dev port
            "https://localhost:4200",     // HTTPS variant
            "http://localhost:5246",      // API's own port (for Swagger)
            "https://localhost:5246"      // HTTPS variant
            // Add more origins here as needed
        )
        .WithMethods("GET")               // Only allow GET requests
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Profile API", Version = "v1", Description = "REST API serving portfolio data" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowedOrigins");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
