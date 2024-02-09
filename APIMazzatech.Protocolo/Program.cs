
using Mazzatech.Data.EF;
using Mazzatech.Data.Repositories;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;
using Mazzatech.Domain.Services;

namespace APIMazzatech.Protocolo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<IProtocoloRepository, ProtocoloRepository>();
            builder.Services.AddScoped<IProtocoloService, ProtocoloService>();
            builder.Services.AddScoped<IDbErrorLoggerRepository, DbErrorLoggerRepository>();
            builder.Services.AddScoped<IDbErrorLoggerService, DbErrorLoggerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
