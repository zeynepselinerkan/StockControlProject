using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StockControlProject.Repository.Abstract;
using StockControlProject.Repository.Concrete;
using StockControlProject.Repository.Context;
using StockControlProject.Service.Abstract;
using StockControlProject.Service.Concrete;

namespace StockControlProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option =>
                option.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore) ;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StockControlContext>(option =>
            {
                option.UseSqlServer("Server=ZSE\\ZSE; Database= StockControlDB; Uid=sa; Pwd=zsezse361;");
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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