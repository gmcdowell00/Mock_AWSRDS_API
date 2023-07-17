using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Mock_AWSRDS_API.Models;

namespace Mock_AWS_API.Repos
{
    public class MyDbContext : DbContext
    {
        
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {            

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Jonin> Jonins { get; set; }
        public DbSet<Genin> Genins { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
