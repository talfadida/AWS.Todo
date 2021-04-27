using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Aws.Todo.Model;

namespace todo
{
    public class ArgumentsParser
    {
        

        public static TodoCommand ParseArgs(string line)
        {
            try
            {
                List<string> args = new List<string>();
                Regex regex = new Regex(@"(""[^""]+"")|\S+");
                foreach (var match in regex.Matches(line))
                    args.Add(match.ToString());

                if (args.Count < 2)
                    return null;
                if (args[0] != "todo")
                    return null;

                switch (args[1])
                {
                    case "add-task":
                        return new TodoCommand() { Operation = EnumOperation.Add,  Title = args[2], IsCompleted = false };
                    case "update-task":
                        return new TodoCommand() { Operation = EnumOperation.Update, Title = args[2], NewTitle = args[3] };
                    case "complete-task":
                        return new TodoCommand() { Operation = EnumOperation.Complete, IsCompleted = true, Title = args[2] };
                    case "undo-task":
                        return new TodoCommand() { Operation = EnumOperation.Undo, IsCompleted = false, Title = args[2] };
                    case "delete-task":
                        return new TodoCommand() { Operation = EnumOperation.Delete, Title = args[2] };
                    case "list-task":
                        return new TodoCommand() { Operation = EnumOperation.List };
                    case "list-completed-task":
                        return new TodoCommand() { Operation = EnumOperation.ListCompleted };
                }
            }
            catch 
            {
               //wrong usage
            }
            return null;
        }

        public static void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine(@"todo add-task ""TASK NAME""");
            sb.AppendLine(@"todo update-task ""TASK NAME"" ""NEW TASK NAME"" ");
            sb.AppendLine(@"todo complete-task ""TASK NAME""");
            sb.AppendLine(@"todo undo-task ""TASK NAME""");
            sb.AppendLine(@"todo delete-task ""TASK NAME""");
            sb.AppendLine(@"todo list-tasks");
            sb.AppendLine(@"todo list-completed-tasks");
            Console.WriteLine(sb.ToString());
        }
    }
}
