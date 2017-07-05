namespace ReadBigFileExcercise
{
    interface IConfiguration
    {
        string DatabaseFilePath { get; }

        bool DeleteDatabaseFile { get; set; }
    }
}
