using APIGateway.Handlers.Seeder; 
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Hosting; 

namespace APIGateway
{
    public class Program
    { 
        public static void Main(string[] args)
        { 
           CreateHostBuilder(args).Build().SeedData().Run(); 
        }
        //.SeedData()
        public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>(); 
        
    }
   

}
