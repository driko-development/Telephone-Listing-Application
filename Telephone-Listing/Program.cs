using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telephone_Listing.Data;
using Telephone_Listing.Services;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.File;

namespace Telephone_Listing
{   
    class Program
    {
        public static IConfigurationRoot configuration;
        private static Command command;
        private static Person person;

        static void Main(string[] args)
        {
            try
            {
                // TODO: Set up log template
                string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u}] [{SourceContext}] {Message}{NewLine}{Exception}";

                Log.Logger = new LoggerConfiguration()
                        .WriteTo.File("log.txt", outputTemplate: logTemplate)
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .CreateLogger();

                if (SetUpArgs(args))
                {
                    MainAsync().Wait();
                }
                else
                {
                    DisplayUsage();
                }    
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occured setting up the arguments for the listing service\n{ex.Message}");
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("     dotnet run ADD <person's name> <telephone num>");
            Console.WriteLine("     dotnet run DEL <person's name>");
            Console.WriteLine("     dotnet run DEL <telephone num>");
            Console.WriteLine("     dotnet run LIST");
            Console.WriteLine("Example:");
            Console.WriteLine("     dotnet run ADD \"Bruce Schneier\" \"(915) 555-4444\"");
            Console.WriteLine("     dotnet run LIST");
        }

        private static bool SetUpArgs(string[] args)
        {
            if (args.Length == 0)
            {
                
                Console.WriteLine("No arguments provided.");

                // No args supplied
                return false;
            }   
            
            if (args.Length > 3)
            {
                Console.WriteLine("Too many arguments provided.");
                // Too many args
                return false;
            }

            // initialize new person
            person = new Person();

            // Validator object used to validate input
            InputValidator validator = new InputValidator();

            // First argument is the mode or command the user wants to run
            var mode = args[0];

            // If the command is add it takes two parameters
            if (mode.Equals("ADD", StringComparison.OrdinalIgnoreCase) && args.Length == 3)
            {
                command = Command.ADD;

                validator.Input = args[1];
                Data.Type firstParam = validator.Validate();

                validator.Input = args[2];
                Data.Type secondParam = validator.Validate();

                if (firstParam.Equals(Data.Type.Invalid) || secondParam.Equals(Data.Type.Invalid))
                {
                    Console.WriteLine($"Name: {firstParam} Phone: {secondParam}");
                    return false;
                }

                if (firstParam.Equals(Data.Type.Name) && secondParam.Equals(Data.Type.PhoneNumber))
                {
                    // set the person name
                    person.Name = args[1];

                    // Set person phone number
                    person.PhoneNumber = args[2];
                }
                else
                {
                    Console.WriteLine("Incorrect usage of ADD command.");
                    return false;
                }
            }

            // If the command is delete it takes one parameter, either phone number or name
            else if (mode.Equals("DEL", StringComparison.OrdinalIgnoreCase) && args.Length == 2)
            {
                command = Command.DEL;

                validator.Input = args[1];
                Data.Type firstParam = validator.Validate();
                
                switch (firstParam)
                {
                    case Data.Type.Name:
                        // Set person name
                        person.Name = args[1];
                        break;
                    case Data.Type.PhoneNumber:
                        // Set person phone number
                        person.PhoneNumber = args[1];
                        break;
                    case Data.Type.Invalid:
                    default:
                        Console.WriteLine("Invalid parameter to DEL command.");
                        return false;
                }
            }

            // If the command is list then no additional parameters are required
            else if (mode.Equals("LIST", StringComparison.OrdinalIgnoreCase) && args.Length == 1)
            {
                command = Command.LIST;
            }

            // Only three supported commands, all others are not allowed
            else
            {
                Console.WriteLine("Invalid input detected.");
                // not valid
                return false;
            }


            // If all checks passed the arguments for our listing service are set up
            return true;
        }

        static async Task MainAsync()
        {
            // Create and configure the service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                await serviceProvider.GetService<ListingService>().Run(command, person);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred inside the listing service.");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(
                LoggerFactory.Create(
                    builder => {
                        builder.AddSerilog(dispose: true);
                    }
                )
            );

            serviceCollection.AddLogging();

            // Build configuration from appsettings.json
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                .AddJsonFile("appsettings.json", false)
                                .Build();

            // Add access to the configuration interface
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Add the telephone listing application
            serviceCollection.AddTransient<ListingService>();
        }
    }
}
