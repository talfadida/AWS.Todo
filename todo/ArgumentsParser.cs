using System;
using System.Collections.Generic;
using System.Text;
using Aws.Todo.Model;

namespace todo
{
    public class ArgumentsParser
    {
        

        public static TodoCommand ParseArgs(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    return null;

                switch (args[0])
                {
                    case "add-task":
                        return new TodoCommand() { Operation = EnumOperation.Add,  Title = args[1], IsCompleted = false };
                    case "update-task":
                        return new TodoCommand() { Operation = EnumOperation.Update, Title = args[1], NewTitle = args[2] };
                    case "complete-task":
                        return new TodoCommand() { Operation = EnumOperation.Complete, IsCompleted = true, Title = args[1] };
                    case "undo-task":
                        return new TodoCommand() { Operation = EnumOperation.Undo, IsCompleted = false, Title = args[1] };
                    case "delete-task":
                        return new TodoCommand() { Operation = EnumOperation.Delete, Title = args[1] };
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
