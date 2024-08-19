
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        public async Task AddProductAsync(ProductDetails productDetails)
        {
            await productCollection.InsertOneAsync(productDetails);
            
        }

        public async Task AddProductsAsync(IList <ProductDetails> productDetails) {

            var bulkOps = new List<WriteModel<ProductDetails>>();
            foreach (var product in productDetails)
            {
                var insertOne = new InsertOneModel<ProductDetails>(product);
                bulkOps.Add(insertOne);
            }
            await productCollection.BulkWriteAsync(bulkOps);
        }

        public async Task DeleteProductAsync(string productId)
        {

            await productCollection.DeleteOneAsync(x => x.Id == productId);
        }

        public async Task<ProductDetails> GetProductDetailByIdAsync(string productId)
        {
            return await productCollection.Find(x => x.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<List<ProductDetails>> ProductListAsync()
        {
            return await productCollection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateProductAsync(string productId, ProductDetails productDetails)
        {
            await productCollection.ReplaceOneAsync(x => x.Id == productId, productDetails);
        }
    }
}
