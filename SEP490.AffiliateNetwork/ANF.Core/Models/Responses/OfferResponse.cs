using ANF.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Models.Responses
{
    public class OfferResponse
    {
        public long Id { get; set; }

        public long CampaignId { get; set; }

        public string? PricingModel { get; set; }

        public string? Note { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public byte[] ConcurrencyStamp { get; set; } = null!;

        public ICollection<Image>? Images { get; set; }
    }
}
