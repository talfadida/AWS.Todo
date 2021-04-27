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

        private static HttpClient httpClient = new HttpClient();
        private static string server_url;

        static async Task Main(string[] args)
        {
             
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            server_url = configuration.GetSection("Settings")["Url"];
            bool.TryParse(configuration.GetSection("Settings")["Debug"], out cfgDebug);


            Console.WriteLine("Welcome to Todo app\r\n");
            ArgumentsParser.PrintHelp();
            while (true)
            {               
                Console.Write("> ");
                string line = Console.ReadLine();

                var command = ArgumentsParser.ParseArgs(line);
                if (command == null)
                {
                    ArgumentsParser.PrintHelp();
                    continue;
                }
                await ExecuteTodoOperation(command);
            }
                        
        }

      
        private static async Task ExecuteTodoOperation(TodoCommand cmd)
        {     
               
            PrintDebug($"Sending operation {cmd.Operation} for {cmd.Title}");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(server_url, content);              
            var bodyOfResponse = await response.Content.ReadAsStringAsync();

            PrintDebug(bodyOfResponse);
                
            var todoResponse = JsonConvert.DeserializeObject<TodoResponse>(bodyOfResponse);
            todoResponse.WriteToConsole();                            
            
        }

        private static void PrintDebug(string str)
        {                              
            if (cfgDebug)
            {
                Console.WriteLine(str);
            }                
        }
         
    }
}
