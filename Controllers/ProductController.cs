using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;

namespace SystemProductOrder.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IProductServies _ProductService;
        private readonly IConfiguration _configuration;
        public ProductController(IProductServies ProductService, IConfiguration configuration)
        {
            _ProductService = ProductService;
            _configuration = configuration;

        }

        [HttpPost]
        [AllowAnonymous]
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ProductInput product)
        {
            try
            {
                _ProductService.AddProduct(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Product added successfully.");
        }

        [HttpPut]
        [AllowAnonymous]
        public IActionResult UpdateProduct( int id )
        {
            try
            {
                _ProductService.UpdateProduct(id );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Product Updated  successfully.");
        }

        [HttpGet]
        public IActionResult GetPagedProducts(string name = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 10)
        {
            try
            {

            var result = _ProductService.GetProducts(name, minPrice, maxPrice, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

          

             // Returns the PagedResult<Product> to the client
        }

        [HttpGet]
        public IActionResult GetProductByid(int id)
        {
            try
            {
                var result=  _ProductService.GetDetailsProductByID(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
