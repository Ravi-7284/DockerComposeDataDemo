
using DockerComposeDataDemo.Data;
using DockerComposeDataDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace DockerComposeDataDemo
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
            builder.Services.AddDbContext<Data.ProductContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope=app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Data.ProductContext>();
                context.Database.Migrate();

                if (!context.Products.Any())
                {
                    context.Products.Add(new Model.Product { Name = "Coca-Cola", Price = 40 });
                    context.Products.Add(new Model.Product { Name = "Pepsi", Price = 30 });
                    context.Products.Add(new Model.Product { Name = "Fanta", Price = 35 });
                    context.SaveChanges();
                }
            }

            app.MapGet("/products", async (ProductContext db) => await db.Products.ToListAsync());

            app.MapPost("/products", async (Product product, ProductContext db) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapControllers();

            app.Run();
        }
    }
}
