using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ReadBigFileExcercise
{
    public class Parser
    {
        private IFileReader _fileReader;
        private IDatabase _database;
        private static Dictionary<string, int> ColumnNamesWithOrder = Project.ColumnNames.ToDictionary<string, int>();
        public Parser(IFileReader fileReader, IDatabase database)
        {
            _fileReader = fileReader;
            _database = database;
        }

        public Result<string> Parse()
        {
            var lines = _fileReader.ReadLines();
            return lines
                .OnSuccess((l) => ParseHeader(l))
                .OnSuccess((meta) => ParseProjectLines(meta.Item1, meta.Item2, ColumnNamesWithOrder, _database));
        }

        private Result<string> ParseProjectLines(IEnumerable<string> lines, int count, IDictionary<string, int> columnNameWithOrder, IDatabase database)
        {
            foreach (var line in lines.Skip(count))
            {
                if (IsActualValue(line).IsSuccess)
                {
                    var result = Project.Create(line.Split('\t'), columnNameWithOrder)
                        .OnSuccess((res) => database.Write(res));
                    if (result.IsFailure)
                        return Result.Fail<string>(result.Error);
                }
            }
            return Result.Ok(_database.FilePath);
        }

        private Result<string> IsActualValue(string line)
        {
            return !string.IsNullOrEmpty(line) && !Regex.IsMatch(line, @"^#") ? Result.Ok(line) : Result.Fail<string>("Not a readable line!");
        }

        private Result<Tuple<IEnumerable<string>, int, IDictionary<string, int>>> ParseHeader(IEnumerable<string> lines)
        {
            try
            {
                int count = 0;
                foreach (var line in lines)
                {
                    if (Regex.IsMatch(line, "^[A-Za-z]")) // MEMO: Here we assume that the first readable text content would be a Header
                    {
                        count++;
                        string[] splitHeader = line.Split('\t');
                        for (int i = 0; i < splitHeader.Length; i++)
                        {
                            ColumnNamesWithOrder[splitHeader[i].Trim()] = i;
                        }
                        break;
                    }

                }
                return count > 0 ?
                    Result.Ok(Tuple.Create<IEnumerable<string>, int, IDictionary<string, int>>(lines, count, ColumnNamesWithOrder)) :
                    Result.Fail<Tuple<IEnumerable<string>, int, IDictionary<string, int>>>("Header could not be found!");
            }
            catch (Exception exception)
            {
                return Result.Fail<Tuple<IEnumerable<string>, int, IDictionary<string, int>>>(exception.Message);
            }
        }
    }
}
