using System.Text.Json.Serialization;
using ProfileApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProfileDataService, ProfileDataService>();
builder.Services.AddScoped<ISkillSortingService, SkillSortingService>();
builder.Services.AddScoped<IWorkExperiencesSortingService, WorkExperiencesSortingService>();
builder.Services.AddScoped<ICertificationsSortingService, CertificationsSortingService>();
builder.Services.AddScoped<IEducationsSortingService, EducationsSortingService>();
builder.Services.AddScoped<ILanguageSkillsSortingService, LanguageSkillsSortingService>();
builder.Services.AddScoped<ISkillAreasSortingService, SkillAreasSortingService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
