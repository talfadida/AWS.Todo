using Aws.Todo.Model;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace todo
{
    public static class Extensions
    {


        private static string GetCompleteChar(bool isCompleted)
        {
            return isCompleted ? "+" : "-";
        }
        public static void WriteToConsole(this TodoResponse todoResponse)
        {
            if (!todoResponse.IsSuccess)
                Console.WriteLine("Error: " + todoResponse.ErrorMessage);

            if (todoResponse.Operation == EnumOperation.List || todoResponse.Operation == EnumOperation.ListCompleted)
            {
                if (todoResponse.TaskList != null && todoResponse.TaskList.Any())
                {
                    var table = new ConsoleTable("NAME", "COMPLETED");
                    todoResponse.TaskList.ForEach(item =>
                    {
                        table.AddRow(item.Title, GetCompleteChar(item.IsCompleted));

                    });
                    table.Write(Format.Minimal);
                }
                else
                {
                    string listMode = todoResponse.Operation == EnumOperation.ListCompleted ? " completed " : " ";
                    Console.WriteLine($"No{listMode}tasks exist");
                }
            } 

        }
    }
}
