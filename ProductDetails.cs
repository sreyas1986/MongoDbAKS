using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace MongoDbAKS
{
    public class ProductDetails
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("ProductName")]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public int ProductStock { get; set; }
        public int ProductPrice { get; set; }


    }
}
