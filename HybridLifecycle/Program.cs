using HybridLifecycle.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddSingleton<HybridConfigurationService>();
services.AddScoped<IConfigurationService>(provider =>
{
    var hybridService = provider.GetRequiredService<HybridConfigurationService>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;
    Guid? scopeId = null;

    if (httpContext.Request.Headers.ContainsKey("X-Scope-ID"))
    {
        if (Guid.TryParse(httpContext.Request.Headers["X-Scope-ID"], out Guid parsedScopeId))
        {
            scopeId = parsedScopeId;
        }
    }

    return hybridService.GetService(scopeId);
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


services.AddHttpContextAccessor();

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
