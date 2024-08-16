
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoDbAKS
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<ProductDetails> productCollection;
        public ProductService(IOptions<ProductDBSettings> productdbsettings )
        {
            var mongoClient = new MongoClient(productdbsettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(productdbsettings.Value.DataBaseName);
            productCollection = mongoDatabase.GetCollection<ProductDetails>(productdbsettings.Value.ProductCollectionName);
        }
        public Task AddProductAsync(ProductDetails productDetails)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public  Task<ProductDetails> GetProductDetailByIdAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductDetails>> ProductListAsync()
        {
            return await productCollection.Find(_ => true).ToListAsync();
        }

        public Task UpdateProductAsync(string productId, ProductDetails productDetails)
        {
            throw new NotImplementedException();
        }
    }
}
