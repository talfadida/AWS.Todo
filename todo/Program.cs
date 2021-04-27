using Aws.Todo.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace todo
{
    class Program
    {

        private static IConfigurationRoot configuration;
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
        }

        private static bool cfgDebug;
        static async Task Main(string[] args)
        {

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            bool.TryParse(configuration.GetSection("Settings")["Debug"], out cfgDebug);

            var command = ArgumentsParser.ParseArgs(args);
            if (command == null)
            {
                ArgumentsParser.PrintHelp();
                return;
            }             
            await ExecuteTodoOperation(command);

        }

      
        private static async Task ExecuteTodoOperation(TodoCommand cmd)
        {
            string server_url = configuration.GetSection("Settings")["Url"];  
            using (HttpClient httpClient = new HttpClient())
            {
               
                PrintDebug($"Sending operation {cmd.Operation} for {cmd.Title}");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(server_url, content);              
                var bodyOfResponse = await response.Content.ReadAsStringAsync();

                PrintDebug(bodyOfResponse);
                
                var todoResponse = JsonConvert.DeserializeObject<TodoResponse>(bodyOfResponse);
                todoResponse.WriteToConsole();

                 
            }
        }

        private static void PrintDebug(string str)
        {                              
            if (cfgDebug)
            {
                Console.WriteLine(str);
            }                
        }


        static async Task RunTestInternal()
        {
            Console.WriteLine("Press any key when you ready");
            Console.ReadKey();
            List<string[]> inputs = new List<string[]>()
            {
                new[]{"list-task"},
                new[]{"add-task", "have something to do"},
                new[]{"add-task", "by dring"},
                new[]{"list-completed-task"},
                new[]{"add-task", "by dring"},
                new[]{"update-task", "by dring", "by drink and food"},
                new[]{"list-task"},
                new[]{"complete-task", "have something to do"},
                new[]{"list-task"},
                new[]{"delete-task", "by drink and food"},
                new[]{"delete-task", "by drink and food"},
                new[]{"list-task"},
                new[]{"add-task", "by drink and food"},
                new[]{"list-task"},
            };

            foreach (var input in inputs)
            {
                var command = ArgumentsParser.ParseArgs(input);
                if (command == null)
                {
                    ArgumentsParser.PrintHelp();
                    return;
                }
                Console.WriteLine("> todo " + string.Join(" ", input));
                await ExecuteTodoOperation(command);
            }
        }

    }
}
