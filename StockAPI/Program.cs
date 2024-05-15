using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var pVAr = Environment.GetEnvironmentVariable("PARAM_VAR") ?? "any";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StocksContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StockDbConnection")));
builder.Services.AddDbContext<masterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("master")));
builder.Services.AddDbContext<DockerStocksContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(pVAr)));
builder.Services.AddDbContext<DockerMasterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DockerMaster")));

var cS = builder.Configuration.GetConnectionString(pVAr);
var app = builder.Build();


    var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DockerStocksContext>();
dbContext.Database.Migrate();


if (app.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
