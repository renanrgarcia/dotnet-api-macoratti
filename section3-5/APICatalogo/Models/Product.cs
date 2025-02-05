using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ApiCatalogo.Validations;

namespace ApiCatalogo.Models;

[Table("Products")]
public class Product : IValidatableObject
{
    [Key]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "The product name cannot be null")]
    [StringLength(20, ErrorMessage = "The product name must be at most 20 characters long"), MinLength(5)]
    // [ToUpperFirstLetter]
    public string Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? Description { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "The price must be between {1} and {2}")]
    public decimal Price { get; set; }
    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime RegisterDate { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            var firstLetter = Name[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
                yield return new ValidationResult("The first letter of the product name must be uppercase.",
                new[] { nameof(Name) }
                );
        }

        if (Stock <= 0)
            yield return new ValidationResult("The stock must be greater than zero.",
            new[] { nameof(Stock) }
            );
    }

}
