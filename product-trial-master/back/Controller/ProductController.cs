using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ProductModel.Model;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
	//liste temp de stockage des products (a remp par bdd)
	private static List<Product> products = new List<Product>();

	//Ajout du context liant l'application à la DB
	private readonly AppDbContext _dbContext;

	//Ajout des dépendance par le constructeur
	public ProductController(AppDbContext dbContext)
	{
        _dbContext = dbContext;

        if (!products.Any())
        { products = _dbContext.Products.ToList(); }
    }

    //GET - tout les products
    //-----------------------------------
    [HttpGet]
	public IActionResult GetAllProducts()
	{
		var products = _dbContext.Products.ToList();
		return Ok(products);
	}

    //GET/local - tout les products en local
    //-----------------------------------
    [HttpGet("local")]
    public IActionResult GetAllProductsLocally()
    {
        return Ok(products);
    }

    //GET{id} - un produit par ID
    //-----------------------------------
    [HttpGet("{id}")]
	public IActionResult GetProductById(int id)
	{
		var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
		if (product == null)
			return NotFound();

		return Ok(product);
	}

    //GET/local/{id} - un produit par ID en local
    //-----------------------------------
    [HttpGet("local/{id}")]
    public IActionResult GetProductByIdLocally(int id)
    {
        var product = products.FirstOrDefault(x => x.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }


    //POST - Créer un Nouveau produit
    //-----------------------------------
    [HttpPost]
	public IActionResult CreateProduct(Product newProduct)
	{
		if (!ModelState.IsValid)
		{ return BadRequest(ModelState); }

        if (!string.IsNullOrEmpty(newProduct.InventoryStatus.ToString()))
        {   newProduct.InventoryStatus = Enum.Parse<InventoryStatus>(newProduct.InventoryStatus.ToString());  }

        newProduct.CreatedAt = DateTime.Now;
        newProduct.UpdatedAt = DateTime.Now;

        _dbContext.Products.Add(newProduct);
		_dbContext.SaveChanges();

        products.Add(newProduct);

        return CreatedAtAction(nameof(GetProductById), new { id =  newProduct.Id }, newProduct);
	}

    //PATCH{id}	- Mise à jour d'un produit
    //-----------------------------------
    [HttpPatch("{id}")]
	public IActionResult UpdateProduct(int id, Product updatedProduct)
	{
        //Liste Locale
        //------------
		var product = products.FirstOrDefault(x =>x.Id == id);
        if (product == null)
        {   return NotFound(); }

        product.Code = updatedProduct.Code != null ? updatedProduct.Code : product.Code;
        product.Name = updatedProduct.Name != null ? updatedProduct.Name : product.Name;
        product.Description = updatedProduct.Description != null ? updatedProduct.Description : product.Description;
        product.Image = updatedProduct.Image != null ? updatedProduct.Image : product.Image;
        product.Category = updatedProduct.Category != null ? updatedProduct.Category : product.Category;
        product.Price = updatedProduct.Price.HasValue ? updatedProduct.Price.Value : product.Price;
        product.Quantity = updatedProduct.Quantity.HasValue ? updatedProduct.Quantity.Value : product.Quantity;
        product.InternalReference = updatedProduct.InternalReference != null ? updatedProduct.InternalReference : product.InternalReference;
        product.ShellId = updatedProduct.ShellId.HasValue ? updatedProduct.ShellId.Value : product.ShellId;
        product.InventoryStatus = updatedProduct.InventoryStatus.HasValue ? updatedProduct.InventoryStatus.Value : product.InventoryStatus;
        product.Rating = updatedProduct.Rating.HasValue ? updatedProduct.Rating.Value : product.Rating;


        //Base de Donnée
        //--------------
        var DbProduct = _dbContext.Products.FirstOrDefault(x => x.Id == id);
        if (DbProduct == null)
        { return NotFound(); }

        DbProduct.Code = updatedProduct.Code != null ? updatedProduct.Code : DbProduct.Code;
        DbProduct.Name = updatedProduct.Name != null ? updatedProduct.Name : DbProduct.Name;
        DbProduct.Description = updatedProduct.Description != null ? updatedProduct.Description : DbProduct.Description;
        DbProduct.Image = updatedProduct.Image != null ? updatedProduct.Image : DbProduct.Image;
        DbProduct.Category = updatedProduct.Category != null ? updatedProduct.Category : DbProduct.Category;
        DbProduct.Price = updatedProduct.Price.HasValue ? updatedProduct.Price.Value : DbProduct.Price;
        DbProduct.Quantity = updatedProduct.Quantity.HasValue ? updatedProduct.Quantity.Value : DbProduct.Quantity;
        DbProduct.InternalReference = updatedProduct.InternalReference != null ? updatedProduct.InternalReference : DbProduct.InternalReference;
        DbProduct.ShellId = updatedProduct.ShellId.HasValue ? updatedProduct.ShellId.Value : DbProduct.ShellId;
        DbProduct.InventoryStatus = updatedProduct.InventoryStatus.HasValue ? updatedProduct.InventoryStatus.Value : DbProduct.InventoryStatus;
        DbProduct.Rating = updatedProduct.Rating.HasValue ? updatedProduct.Rating.Value : DbProduct.Rating;

        // Mise à jour de la date de modification
        product.UpdatedAt = DateTime.Now;
        DbProduct.UpdatedAt = DateTime.Now;

        // Sauvegarder les modifications en base de données
        _dbContext.SaveChanges();

        return Ok(product);
    }

    private static object   GetDefaultValue(Type type)
    {   return type.IsValueType ? Activator.CreateInstance(type) : null;    }

    //DELETE - Supprimer un produit
    //-----------------------------------
    [HttpDelete("{id}")]
	public IActionResult DeleteProduct(int id)
	{
        //Local
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        { return NotFound(); }

        products.Remove(product);

        //DB
        var DbProduct = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (DbProduct == null)
        { return NotFound(); }

        _dbContext.Products.Remove(DbProduct);
        _dbContext.SaveChanges();

        return NoContent();
	}

    /*	GET - TEST - Trigger d'une erreur
    **	---------------------------------	*/
    [HttpGet("test-error-trigger")]
    public IActionResult TriggerTestError()
    { throw new Exception("ERREUR TEST !"); }
    /*	---------------------------------	*/
}
