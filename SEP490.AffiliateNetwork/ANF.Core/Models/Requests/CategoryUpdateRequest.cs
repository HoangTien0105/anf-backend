using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CategoryUpdateRequest
    {
        [Required(ErrorMessage = "Category's name is required!")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
