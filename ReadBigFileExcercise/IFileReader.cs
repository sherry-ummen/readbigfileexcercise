using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace ReadBigFileExcercise
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> ReadLines();
    }
}