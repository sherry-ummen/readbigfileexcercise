using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace ReadBigFileExcercise
{
    public interface IDatabase
    {
        string FilePath { get; }
        Result Write(Project project);
        Result<IEnumerable<Project>> RetrieveResults(SearchCriteria searchCriteria);
        Result Close();
    }
}