using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Models.Responses
{
    public class CampaignPubDetailedResponse
    {
        public long Id { get; init; }

        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public decimal? Balance { get; init; }

        public string ProductUrl { get; init; } = null!;

        public string? TrackingParams { get; init; }

        public long? CategoryId { get; init; }

        public string? CategoryName { get; init; }

        public ICollection<string> CampImages { get; set; } = new List<string>();

        public ICollection<OfferResponse> Offers { get; set; } = new List<OfferResponse>();
    }
}
