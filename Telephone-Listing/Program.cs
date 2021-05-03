using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telephone_Listing.Data;
using Telephone_Listing.Services;

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
                if (SetUpArgs(args))
                {
                    Console.WriteLine("You made it into the program");
                    //MainAsync().Wait();
                }
                else
                {
                    DisplayUsage();
                }    
            }
            catch
            {

            }
        }

        private static void DisplayUsage()
        {
            throw new NotImplementedException();
        }

        private static bool SetUpArgs(string[] args)
        {
            if (args.Length == 0)
            {
                
                Console.WriteLine("No args");
                // No args supplied
                return false;
            }   
            
            if (args.Length > 3)
            {
                Console.WriteLine("Too many args");
                // Too many args
                return false;
            }

            person = new Person();
            InputValidator validator = new InputValidator();

            var mode = args[0];
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
                    Console.WriteLine("Need args to add");
                    return false;
                }
            }
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
                        return false;
                }
            }
            else if (mode.Equals("LIST", StringComparison.OrdinalIgnoreCase) && args.Length == 1)
            {
                command = Command.LIST;
            }
            else
            {
                // not valid
                return false;
            }

            return true;
        }

        static async Task MainAsync()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetService<ListingService>().Run(command, person);
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                .AddJsonFile("appsettings.json", false)
                                .Build();

            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            serviceCollection.AddTransient<ListingService>();
        }
    }
}
