using Microsoft.AspNetCore.Mvc;
using HelloWebApi.Models;


namespace HelloWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Tshirt", Price = 10.0m },
            new Product { Id = 2, Name = "Jean", Price = 20.0m },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get([FromQuery] string name)
        {
            var products = string.IsNullOrEmpty(name) ? _products : _products.Where(p => p.Name.Contains(name)).ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Urun bulunamadi" });
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Urun bulunamadi" });
            }
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Urun bulunamadi" });
            }
            _products.Remove(product);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Urun bulunamadi" });
            }
            if (updatedProduct.Name != null)
            {
                product.Name = updatedProduct.Name;
            }
            if (updatedProduct.Price != 0)
            {
                product.Price = updatedProduct.Price;
            }
            return NoContent();
        }
    }
}
