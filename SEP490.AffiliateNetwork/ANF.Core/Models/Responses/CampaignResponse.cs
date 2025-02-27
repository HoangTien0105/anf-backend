using ANF.Core.Enums;
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
    public class CampaignResponse
    {
        public long Id { get; set; }

        public long AdvertiserId { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Budget { get; set; }

        public double Balance { get; set; }

        public string ProductUrl { get; set; } = null!;

        public string? TrackingParams { get; set; }

        public long? CategoryId { get; set; }

        public string Status { get; set; } = null!;
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public UserResponse Advertiser { get; set; } = null!;

        public Category? Category { get; set; }
        public ICollection<OfferResponse>? Offers { get; set; }

        public ICollection<Image>? Images { get; set; }
    }
}
