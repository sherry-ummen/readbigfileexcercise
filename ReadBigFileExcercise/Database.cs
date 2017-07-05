using System;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using LiteDB;
using System.Linq;
using System.IO;

namespace ReadBigFileExcercise
{
    class Database : IDatabase
    {
        private LiteDatabase _litedb;
        private LiteCollection<Project> _collection;
        private string _path;

        public string FilePath => _path;

        public Database(IConfiguration configuration)
        {
            if (configuration.DeleteDatabaseFile && File.Exists(configuration.DatabaseFilePath))
                File.Delete(configuration.DatabaseFilePath);
            if (!Directory.Exists(Directory.GetParent(configuration.DatabaseFilePath).FullName))
                Directory.CreateDirectory(Directory.GetParent(configuration.DatabaseFilePath).FullName);
            if (!File.Exists(configuration.DatabaseFilePath))
                File.Create(configuration.DatabaseFilePath).Close();
            _path = configuration.DatabaseFilePath;
            _litedb = new LiteDatabase(configuration.DatabaseFilePath);
            _collection = _litedb.GetCollection<Project>("Projects");
        }

        public Result Write(Project project)
        {
            try
            {
                _collection.Upsert(project);
                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }

        public Result<IEnumerable<Project>> RetrieveResults(SearchCriteria searchCriteria)
        {
            try
            {
                if (searchCriteria.Ids.Count() > 0)
                {
                    var result = _collection.Find(Query.In("_id", searchCriteria.Ids.Select(x => new BsonValue(x))));
                    if (searchCriteria.SortByDate)
                        return Result.Ok(result.OrderBy(x => x.StartDate).Select(x => x));
                }
                else if (searchCriteria.SortByDate)
                {
                    return Result.Ok(_collection.FindAll().OrderBy(x => x.StartDate).Select(x => x));
                }
                return Result.Ok(_collection.FindAll());
            }
            catch (Exception exception)
            {
                return Result.Fail<IEnumerable<Project>>(exception.Message);
            }
        }

        public Result Close()
        {
            _litedb?.Dispose();
            return Result.Ok();
        }
    }
}
