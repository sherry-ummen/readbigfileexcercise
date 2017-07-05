using CommandLine;
using System.Collections.Generic;

namespace ReadBigFileExcercise
{
    public class Options
    {
        [Option('s', "sort", Required = false, HelpText = "Sorts the result by Start Date field")]
        public bool SortByStartDate { get; set; }

        [Option('p', "projectids", Required = false, HelpText = "Array of project ids, in case of single id then just give one id", Default = new int[] { })]
        public IEnumerable<int> ProjectIds { get; set; }

        [Option('i', "input", Required = false, HelpText = "Input file to read.")]
        public string InputFilePath { get; set; }

        [Option('g', "generatebigfile", Required = false, HelpText = "Generate big file with random data")]
        public bool GenerateBigFile { get; set; }

        [Option('v', "generateinvalidvalue", Required = false, HelpText = "Generate invalid values for the random data")]
        public bool GenerateIvalidValues { get; set; }

        [Option('r', "order", Required = false, HelpText = "Order of the columns for the generated file", Default = new[] { 0, 1, 2, 3, 4, 5, 6, 7 })]
        public IEnumerable<int> OrderOfColumn { get; set; }

        [Option('o', "output", Required = false, HelpText = "File path for the file to be generated.")]
        public string OutputFilePath { get; set; }

        [Option('t', "totalrows", Required = false, HelpText = "Total number of rows in the generated file.", Default = 100)]
        public int TotalRows { get; set; }

        [Option('x', "usedatabase", Required = false, HelpText = "Uses previously generated database file if it exists")]
        public bool UseDatabase { get; set; }
    }
}
