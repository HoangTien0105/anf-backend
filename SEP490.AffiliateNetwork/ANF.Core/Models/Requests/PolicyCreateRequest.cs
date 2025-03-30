using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PolicyCreateRequest
    {

        [BindProperty(Name = "Header")]
        [Required(ErrorMessage = "Header is requied.")]
        public string Header { get; set; } = null!;

        [BindProperty(Name = "Description")]
        [Required(ErrorMessage = "Description is requied.")]
        public string? Description { get; set; }


    }
}
