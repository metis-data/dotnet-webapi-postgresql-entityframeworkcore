using Microsoft.EntityFrameworkCore;
using dotnet_webapi_postgresql_entityframeworkcore.Models;

public class Program
{
      public static void Main(string[] args) {
          var builder = WebApplication.CreateBuilder(args);

          builder.Services.AddCors();
          builder.Services.AddControllers();
          builder.Services.AddDbContext<ImdbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
          builder.Services.AddEndpointsApiExplorer();
          builder.Services.AddSwaggerGen();

          var app = builder.Build();

          app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
          app.UseSwagger();
          app.UseSwaggerUI();
          app.MapControllers();

          app.Run();
      }
}


