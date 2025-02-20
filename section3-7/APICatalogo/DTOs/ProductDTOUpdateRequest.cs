using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs
{
    public class ProductDTOUpdateRequest
    {
        [Range(1, 9999, ErrorMessage = "The field ProductId must be between 1 and 9999.")]
        public float Stock { get; set; }

        public DateTime RegisterDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RegisterDate <= DateTime.Now)
                yield return new ValidationResult("The RegisterDate field must be greater than the current date.",
                new[] { nameof(RegisterDate) }
                );
        }
    }
}
