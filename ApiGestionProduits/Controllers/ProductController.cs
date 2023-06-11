using ApiGestionProduits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionProduits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _productContext;
        public ProductController(ProductContext productContext)
        {
            _productContext = productContext;
        }
      //Get All Product from database
        //Get All Product from database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct()
        {
            if (_productContext.Products == null)
            {
                return NotFound();
            }
            return await _productContext.Products.ToListAsync();
        }
        //Get Product by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            if (_productContext.Products == null)
            {
                return NotFound();
            }
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        //Add a new product
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _productContext.Products.Add(product);
            await _productContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllProduct), new { id = product.Id }, product);
        }

        //Update a product
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _productContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _productContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductVal(id))
                {
                    return NotFound();
                }
            }

            return Ok();
        }
        private bool ProductVal(int id)
        {
            return(_productContext.Products?.Any(p => p.Id == id)).GetValueOrDefault();
        }

        //Delete a product
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (_productContext.Products == null)
            {
                return NotFound();
            }
            var produit = await _productContext.Products.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            _productContext.Products.Remove(produit);
            await _productContext.SaveChangesAsync();
            return Ok();

        }
        
    }
}
