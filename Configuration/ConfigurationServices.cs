using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mock_AWS_API.Interface;
using Mock_AWS_API.Repos;
using Mock_AWS_API.Services;


namespace Mock_AWS_API.Configuration
{
    public class ConfigurationServices
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // Dependency Injection
            // Add Datacontext
            //services.AddSingleton<MongoDbContext>();
                     
            // Services
            services.AddScoped<MockService>();

            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(configuration.GetValue<string>("MongoDB:ConnectionString:DefaultConnection")));
           


            // Repors
            //services.AddScoped<IFilesRepo, FilesRepo>();            
        }
    }
}
