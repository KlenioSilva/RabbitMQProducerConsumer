
using Mazzatech.Data.EF;
using Mazzatech.Data.Repositories;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;
using Mazzatech.Domain.Services;

namespace APIMazzatech.Authenticate
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
            builder.Services.AddScoped<AuthenticateContext>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IIssuerUserSecretKeyTokenRepository, IssuerUserSecretKeyTokenRepository>();
            builder.Services.AddScoped<IIssuerUserSecretKeyTokenService, IssuerUserSecretKeyTokenService>();
            builder.Services.AddScoped<IIssuerSecretKeyRepository, IssuerSecretKeyRepository>();
            builder.Services.AddScoped<IIssuerSecretKeyService, IssuerSecretKeyService>();

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
