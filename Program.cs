// See https://aka.ms/new-console-template for more information
// https://www.youtube.com/watch?v=l6s7AvZx5j8&t=1904s
using static System.Net.WebRequestMethods;

using System;
using System.Collections.Generic;
//using ConsoleUI.WithoutGenerics;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();

            DemonstrateTextFileStorage();

            Console.WriteLine("Hello, World!");
            Console.Write("Press enter to shut down...");
            Console.ReadLine();
        }
        private static void DemonstrateTextFileStorage()
        {
            List<Person> people = new();
            List<LogEntry> logs = new();
            PopulateLists(people, logs);



            System.Console.WriteLine("\nNew way of doing things Generics");
            string peopleFileGenerics = @"C:\Temp\peopleGenerics.csv";
            string logFileGenerics = @"C:\Temp\logsGenerics.csv";
            
            GenericTextFileProcessor.SaveToTextFile<Person>(people, peopleFileGenerics);
            var newPeopleGenerics = GenericTextFileProcessor.LoadFromTextFile<Person>(peopleFileGenerics);
            foreach (var p in newPeopleGenerics)
            {
                Console.WriteLine($"{p.FirstName} {p.LastName} (IsAlive = {p.IsAlive})");
            }

            GenericTextFileProcessor.SaveToTextFile<LogEntry>(logs, logFileGenerics);
            var newLogsGenerics = GenericTextFileProcessor.LoadFromTextFile<LogEntry>(logFileGenerics);
            foreach (var log in newLogsGenerics)
            {
                Console.WriteLine($"{log.ErrorCode}: {log.Message} at {log.TimeOfEvent.ToShortTimeString()}");
            }


            System.Console.WriteLine("\nOld way of doing things - non - generics");
            string peopleFile = @"C:\Temp\people.csv";
            string logFile = @"C:\Temp\logs.csv";
            
            OriginalTextFileProcessor.SavePeople(people, peopleFile);
            var newPeople = OriginalTextFileProcessor.LoadPeople(peopleFile);
            foreach (var p in newPeople)
            {
                Console.WriteLine($"{p.FirstName}{p.LastName} (IsAlive + {p.IsAlive})");
            }

            OriginalTextFileProcessor.SaveLogs(logs, logFile);
            var newLogs = OriginalTextFileProcessor.LoadLogs(logFile);
            foreach (var log in newLogs)
            {
                Console.WriteLine($"{log.ErrorCode}{log.Message} (IsAlive + {log.TimeOfEvent.ToShortTimeString()})");
            }



        }
        private static void PopulateLists(List<Person> people, List<LogEntry> logs)
        {
            people.Add(new Person { FirstName = "Steve", LastName = "Jones" });
            people.Add(new Person { FirstName = "John", LastName = "Doe", IsAlive = false });
            people.Add(new Person { FirstName = "Joe", LastName = "Frank" });

            logs.Add(new LogEntry { Message = "Minor Error Don't worry", ErrorCode = 0035 });
            logs.Add(new LogEntry { Message = "PSU on fire", ErrorCode = 29 });
            logs.Add(new LogEntry { Message = "Hair on Fire", ErrorCode = 57 });
        }

    }
}

