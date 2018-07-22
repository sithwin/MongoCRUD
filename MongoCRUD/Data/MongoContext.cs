using MongoCRUD.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCRUD.Data
{
    public class MongoContext
    {
        public IMongoDatabase mongoDatabase;

        public MongoContext()
        {
            var mongoCleint = new MongoClient("mongodb://localhost:27017");
            mongoDatabase = mongoCleint.GetDatabase("CustomerDB");
        }

        public IMongoCollection<Rental> Rentals {
            get
            {
                return mongoDatabase.GetCollection<Rental>("Rentals");
            }
        }
    }
}
