using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MongoDbAKS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;

        }
        [HttpGet]
        public async Task<List<ProductDetails>> GetProduct(){
                return await  productService.ProductListAsync();

        }
        [HttpGet("{productId:minlength(4)}")]
        public async Task<ProductDetails> GetProduct(string productId) { 
            return await productService.GetProductDetailByIdAsync(productId);
        }
        [HttpPost]
        public async Task<IActionResult> Post(ProductDetails productDetails) {

            await productService.AddProductAsync(productDetails);
            return CreatedAtAction(nameof(GetProduct), new
            {
                id = productDetails.Id
            }, productDetails);

        }
        [HttpPut("{productId:minlength(4)}")]
        public async Task<IActionResult> Update(string productId,ProductDetails productDetails) {

            var productDetail =await productService.GetProductDetailByIdAsync(productId);
            if (productDetail is null) {

                return NotFound();
            }
            productDetails.Id = productDetail.Id;
            await productService.UpdateProductAsync(productId, productDetails);
            return Ok();

        }
        [HttpDelete("{productId:minlength(4)}")]
        public async Task<IActionResult> Delete(string productId) {

            var productDetail = await productService.GetProductDetailByIdAsync(productId);
            if(productDetail is null)
            {

                return NotFound();
            }
            await productService.DeleteProductAsync(productId);
            return Ok();
        }



    }
}
