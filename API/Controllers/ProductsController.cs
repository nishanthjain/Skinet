using System;
using System.Collections;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductsController : ControllerBase
{
    public StoreContext context { get; }
    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        return await context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductByID(int id)
    {
        var product = await context.Products.FindAsync(id);
        return product == null ? NotFound() : product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.ID || !ProductExists(id))
            return BadRequest("The product doesnot exists.");

        context.Entry(product).State = EntityState.Modified;
        context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        context.Products.Remove(product);
        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.ID == id);
    }
}
