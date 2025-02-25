using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Response model for subscriptions
    /// </summary>
    public class SubscriptionResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }

        public string? Duration { get; set; }

    }
}
