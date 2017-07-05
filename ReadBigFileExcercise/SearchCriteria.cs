using System.Collections.Generic;

namespace ReadBigFileExcercise
{
    public class SearchCriteria
    {
        public bool SortByDate { get; set; }
        public IEnumerable<int> Ids { get; set; } 
    }
}
