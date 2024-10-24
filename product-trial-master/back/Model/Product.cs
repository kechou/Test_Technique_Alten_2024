using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductModel.Model
{
    public enum InventoryStatus
    {
        INSTOCK,
        LOWSTOCK,
        OUTOFSTOCK
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Un code produit est obligatoire.")]
        public string? Code { get; set; }
        [Required(ErrorMessage = "Un nom de produit est obligatoire.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Une catégorie de produit est obligatoire.")]
        public string? Category { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "le prix doit être positif.")]
        public decimal? Price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "La quantité de produit ne peut être négative.")]
        public int? Quantity { get; set; }
        [Required(ErrorMessage = "Le produit doit être rattaché à une référence interne.")]
        public string? InternalReference { get; set; }
        [Required(ErrorMessage = "Un shellId est obligatoire.")]
        public int? ShellId { get; set; }
        [Required(ErrorMessage = "Le Status de l'inventaire est obligatoire: INSTOCK, LOWSTOCK, OUTOFSTOCK")]
        public InventoryStatus? InventoryStatus { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "la note du produit ne peut être négative.")]
        public double? Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Product() {   /*Pour désérialiser le json*/   }
    }    
}
