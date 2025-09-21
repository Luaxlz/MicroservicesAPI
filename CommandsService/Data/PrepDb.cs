using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            SeedData(context, isProduction);
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Error applying migrations: {ex.Message}");
                }
            }

            SeedPlatforms(context);
            
            SeedCommands(context);
        }

        private static void SeedPlatforms(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Platforms data...");
            }
            else
            {
                Console.WriteLine("--> Platforms already exist, skipping seed");
            }
        }

        private static void SeedCommands(AppDbContext context)
        {
            if (!context.Commands.Any())
            {
                Console.WriteLine("--> Seeding Commands data...");

                var dotNetPlatform = context.Platforms.FirstOrDefault(p => p.Name == ".NET");
                var sqlPlatform = context.Platforms.FirstOrDefault(p => p.Name == "SQL Server");
                var k8sPlatform = context.Platforms.FirstOrDefault(p => p.Name == "Kubernetes");

                var commands = new List<Command>();

                if (dotNetPlatform != null)
                {
                    commands.AddRange(new[]
                    {
                        new Command() 
                        { 
                            HowTo = "Build a .NET Project", 
                            CommandLine = "dotnet build", 
                            PlatformId = dotNetPlatform.Id,
                            Platform = dotNetPlatform
                        },
                        new Command() 
                        { 
                            HowTo = "Run a .NET Project", 
                            CommandLine = "dotnet run", 
                            PlatformId = dotNetPlatform.Id,
                            Platform = dotNetPlatform
                        }
                    });
                }

                if (sqlPlatform != null)
                {
                    commands.Add(new Command() 
                    { 
                        HowTo = "Connect to SQL Server", 
                        CommandLine = "sqlcmd -S localhost -d mydb", 
                        PlatformId = sqlPlatform.Id,
                        Platform = sqlPlatform
                    });
                }

                if (k8sPlatform != null)
                {
                    commands.AddRange(new[]
                    {
                        new Command() 
                        { 
                            HowTo = "Get all pods", 
                            CommandLine = "kubectl get pods", 
                            PlatformId = k8sPlatform.Id,
                            Platform = k8sPlatform
                        },
                        new Command() 
                        { 
                            HowTo = "Get services", 
                            CommandLine = "kubectl get services", 
                            PlatformId = k8sPlatform.Id,
                            Platform = k8sPlatform
                        }
                    });
                }

                if (commands.Any())
                {
                    context.Commands.AddRange(commands);
                    context.SaveChanges();
                    Console.WriteLine($"--> {commands.Count} Commands seeded successfully!");
                }
            }
            else
            {
                Console.WriteLine("--> Commands already exist, skipping seed");
            }
        }
    }
}