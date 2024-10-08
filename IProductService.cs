﻿using MongoDB.Driver;

namespace MongoDbAKS
{
    
        public interface IProductService
        {
            public Task<List<ProductDetails>> ProductListAsync();
            public Task<ProductDetails> GetProductDetailByIdAsync(string productId);
            public Task AddProductAsync(ProductDetails productDetails);
            public Task AddProductsAsync(IList<ProductDetails> products);
            public Task UpdateProductAsync(string productId, ProductDetails productDetails);
            public Task DeleteProductAsync(String productId);
        }
    
}
