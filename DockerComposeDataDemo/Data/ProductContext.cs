using DockerComposeDataDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace DockerComposeDataDemo.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
