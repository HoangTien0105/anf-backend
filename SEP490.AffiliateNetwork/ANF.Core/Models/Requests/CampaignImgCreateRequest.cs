using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Models.Requests
{
    public class CampaignImgCreateRequest
    {
        public long? CampaignId { get; set; }
        [Required(ErrorMessage = "ImageUrl is required.")]
        public string ImageUrl { get; set; } = null!;
    }
}
