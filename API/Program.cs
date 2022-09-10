using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;
using Core.Identity;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {

                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, loggerFactory);


                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

                    //Attempt to create database
                    await identityContext.Database.MigrateAsync();
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);







                }
                catch (Exception ex)
                {
                    //Creating an instance of a logger
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred while migrations ");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
