using Purin.PackageManager.Server.Services;
using SafeList;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

StartupService.Configure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<IpSafeListMiddleware>(ConfigService.SafeList.PermittedIpAddresses, new List<string> { HttpMethod.Get.Method });

app.UseAuthorization();

app.MapControllers();

app.Run();
