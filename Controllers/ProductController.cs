using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using ExcelDataReader;
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

        [HttpPost("upload")]
        public async Task<IActionResult>  Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            DataSet dataSet;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                }
            }

            var dataTable = dataSet.Tables[0];
            var result = new List<Dictionary<string, object>>();

            var productDet = new List<ProductDetails>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {

                
                var productDetail = new ProductDetails();
                productDetail.Id = Convert.ToString(row[0]);
                productDetail.ProductName = Convert.ToString(row[1]);
                productDetail.ProductDescription = Convert.ToString(row[2]);
                productDetail.ProductPrice = Convert.ToInt16(row[3].ToString());
                productDetail.ProductStock = Convert.ToInt16(row[4].ToString());
                productDet.Add(productDetail);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
               
            }
            await productService.AddProductsAsync(productDet);
            return CreatedAtAction(nameof(GetProduct),null);
        }

    }
}
