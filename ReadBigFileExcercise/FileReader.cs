using System.Collections.Generic;
using CSharpFunctionalExtensions;
using System.IO;

namespace ReadBigFileExcercise
{
    public class FileReader : IFileReader
    {
        private string _path;

        public FileReader(string path)
        {
            _path = path;
        }

        public Result<IEnumerable<string>> ReadLines()
        {
            return Result.Ok(File.ReadLines(_path));
        }
    }
}
