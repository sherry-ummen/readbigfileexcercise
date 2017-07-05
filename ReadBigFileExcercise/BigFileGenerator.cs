using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReadBigFileExcercise
{
    public static class BigFileGenerator
    {
        private static Random rnd = new Random();

        public static Result<string> Generate(Options options)
        {
            try
            {
                if (!options.GenerateBigFile)
                {
                    return Result.Ok(options.InputFilePath);
                }
                Generate(options.OutputFilePath, options.TotalRows, options.OrderOfColumn, options.GenerateIvalidValues);
                return Result.Ok(options.OutputFilePath);
            }
            catch (Exception exception)
            {
                return Result.Fail<string>(exception.Message);
            }
        }


        public static void Generate(string filePath, int numberOfLines, IEnumerable<int> columnOrder, bool invalidValues)
        {
            if (string.IsNullOrEmpty(filePath)) throw new Exception("File path cannot be empty");
            if (File.Exists(filePath))
                File.Delete(filePath);
            if (columnOrder == null || columnOrder.Count() < Project.ColumnNames.Count())
                columnOrder = Enumerable.Range(0, Project.ColumnNames.Count());
            var columnNames = columnOrder.Select(X => Project.ColumnNames.ToArray()[X]).ToArray();
            using (var file = File.CreateText(filePath))
            {
                file.WriteLine(GenerateHeader(columnNames));
                foreach (var line in GenerateColumnValues(numberOfLines, columnNames, invalidValues))
                {
                    file.WriteLine(line);
                }
            }
        }

        private static string GenerateHeader(IEnumerable<string> columnNames)
        {
            return string.Join("\t", columnNames);
        }

        private static IEnumerable<string> GenerateColumnValues(int numberOrLines, IEnumerable<string> columnNames, bool invalidValues)
        {
            var currencies = new[] { "EUR", "USD", "INR", "NULL" };

            foreach (var index in Enumerable.Range(1, numberOrLines))
            {
                List<string> columnValues = new List<string>();
                foreach (var column in columnNames)
                {
                    switch (column)
                    {
                        // MEMO: Not nice to put the names of the column directly, but currently I do not have better idea.
                        case "Project": columnValues.Add(index.ToString()); break;
                        case "Description": columnValues.Add(Guid.NewGuid().ToString()); break;
                        case "Start date": columnValues.Add(DateTime.UtcNow.AddDays(rnd.Next(90)).ToString("yyyy-MM-dd HH:mm:ss.fff")); break;
                        case "Category": columnValues.Add(Guid.NewGuid().ToString().Substring(0, 4)); break;
                        case "Responsible": columnValues.Add(Guid.NewGuid().ToString().Substring(0, 6)); break;
                        case "Savings amount": columnValues.Add(index % 3 == 0 ? "NULL" : GetRandomDoubleNumber(4880.199567, 11689.322459).ToString()); break;
                        case "Currency": columnValues.Add(currencies[rnd.Next(0, currencies.Length)]); break;
                        case "Complexity": columnValues.Add(invalidValues && index % 5 == 0 ? "INVALID" : ((Complexity)rnd.Next(Enum.GetNames(typeof(Complexity)).Length)).ToString()); break;
                        default: throw new Exception("Unsupported column name " + column);
                    }
                }
                yield return string.Join("\t", columnValues);
            }
        }

        private static double GetRandomDoubleNumber(double minimum, double maximum)
        {
            return rnd.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}
