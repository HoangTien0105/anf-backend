using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
