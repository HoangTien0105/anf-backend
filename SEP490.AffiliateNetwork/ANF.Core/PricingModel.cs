using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core
{
    public class PricingModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public static class PricingModelConstant
    {
        public static readonly List<PricingModel> pricingModels = new()
    {
        new PricingModel { Id = 1, Name = "CPC", Description = "Cost per click" },
        new PricingModel { Id = 2, Name = "CPV", Description = "Cost per view" },
        new PricingModel { Id = 3, Name = "CPA", Description = "Cost per action" },
    };
    }

}
