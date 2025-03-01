using ANF.Core;
using ANF.Core.Commons;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PricingModelController : BaseApiController
    {
        /// <summary>
        /// Get all pricing models
        /// </summary>
        /// <returns></returns>
        [HttpGet("pricing-models")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPricingModels()
        {
            var pricingModels = PricingModelConstant.pricingModels;
            return Ok(new ApiResponse<List<PricingModel>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = pricingModels
            });
        }

        /// <summary>
        /// Get pricing model by id
        /// </summary>
        /// <param name="id">Pricing model id</param>
        /// <returns></returns>
        [HttpGet("pricing-models/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPricingModel(long id)
        {
            var pricingModel = PricingModelConstant.pricingModels.Where(e => e.Id == id).FirstOrDefault();
            return Ok(new ApiResponse<PricingModel>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = pricingModel
            });
        }
    }
}
