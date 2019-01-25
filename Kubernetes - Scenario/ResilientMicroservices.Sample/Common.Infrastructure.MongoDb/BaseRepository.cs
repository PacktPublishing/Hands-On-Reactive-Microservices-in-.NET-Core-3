using System;
using System.Collections.Generic;
using System.Text;
using EnsureThat;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Common.Infrastructure.MongoDb
{
    public class BaseRepository
    {
        protected readonly IMongoDatabase MongoDatabase;

        protected BaseRepository(MongoDbSettings settings)
        {
            Ensure.That(settings).IsNotNull();
            Ensure.String.IsNotNullOrEmpty(settings.ServerConnection);
            Ensure.String.IsNotNullOrEmpty(settings.Database);

            var client = new MongoClient(settings.ServerConnection);

            MongoDatabase = client.GetDatabase(settings.Database,
                new MongoDatabaseSettings
                {
                    GuidRepresentation = GuidRepresentation.Standard
                });
        }
    }
}
