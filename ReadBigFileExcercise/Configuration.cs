using System;
using System.IO;

namespace ReadBigFileExcercise
{
    class Configuration : IConfiguration
    {
        public Configuration(bool useDatabaseFile = false)
        {
            DeleteDatabaseFile = !useDatabaseFile;
        }
        public string DatabaseFilePath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),"ProjectsParser","1.0.0","projectsparser.db"); } }

        public bool DeleteDatabaseFile { get; set; }
    }
}
