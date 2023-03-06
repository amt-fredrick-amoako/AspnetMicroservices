using Catalog.Api.Entities;
using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration _configuration;
        public CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var databse = client.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = databse.GetCollection<Product>(_configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
