using CommandLine;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace ReadBigFileExcercise
{

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayError("Please pass required arguments");
                return;
            }
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args)
                          .WithParsed(Execute)
                          .WithNotParsed(errors => DisplayParserErrors(errors));
        }

        private static void Execute(Options options)
        {
            var database = new Database(new Configuration(options.UseDatabase));
            (options.UseDatabase ? Result.Ok("") : BigFileGenerator.Generate(options))
                .OnSuccess(result => options.UseDatabase ? Result.Ok("") : new Parser(new FileReader(result), database).Parse())
                .OnSuccess(() => database.RetrieveResults(new SearchCriteria() { Ids = options.ProjectIds, SortByDate = options.SortByStartDate }))
                .OnSuccess((results) => DisplayResults(Project.ColumnNames, results))
                .OnBoth(result => result.IsSuccess ? Result.Ok() : DisplayError(result.Error))
                .OnBoth((result) => database.Close());

        }

        private static void DisplayParserErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("Command line parser Error:");
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private static Result DisplayError(string error)
        {
            Console.WriteLine("Failed with error: " + error);
            return Result.Ok();
        }

        private static Result DisplayResults(IEnumerable<string> headers, IEnumerable<Project> results)
        {
            try
            {
                Console.WriteLine(string.Join("\t", headers));
                foreach (var project in results)
                {
                    Console.WriteLine(project);
                }
                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }

        }
    }


}
