using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CategoryCreateRequest
    {
        [Required(ErrorMessage = "Category's name is required!", AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
    }
}
