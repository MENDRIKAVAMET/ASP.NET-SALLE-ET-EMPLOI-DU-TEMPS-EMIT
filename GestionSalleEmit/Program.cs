using GestionSalleEmit.Data;
using GestionSalleEmit.Services;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enregistrement des services pour l'injection de dépendances
            builder.Services.AddScoped<IEmploiDuTempsService, EmploiDuTempsService>();
            builder.Services.AddScoped<ISallesService, SallesService>(); // L'enregistrement du service Salles ajouté ici

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
        }
    }
}