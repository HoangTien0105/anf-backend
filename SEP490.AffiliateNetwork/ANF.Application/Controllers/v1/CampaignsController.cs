using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Models.Entities;
using ANF.Infrastructure;
using ANF.Core.Services;
using Asp.Versioning;
using ANF.Core.Models.Responses;
using ANF.Core.Models.Requests;
using ANF.Core.Commons;
using ANF.Service;
using Microsoft.AspNetCore.Authorization;

namespace ANF.Application.Controllers.v1
{
    public class CampaignsController(ICampaignService campaignService) : BaseApiController
    {
        private readonly ICampaignService _campaignService = campaignService;

        /// <summary>
        /// Get all campaigns
        /// </summary>
        /// <param name="request">Pagination data</param>
        /// <returns></returns>
        [HttpGet("campaigns")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaigns([FromQuery] PaginationRequest request)
        {
            var campaigns = await _campaignService.GetCampaigns(request);
            return Ok(new ApiResponse<PaginationResponse<CampaignResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        /// <summary>
        /// Get all campaigns with offers
        /// </summary>
        /// <param name="request">Pagination data</param>
        /// <returns></returns>
        [HttpGet("campaigns/offers")]
        [MapToApiVersion(1)]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaignsWithOffers([FromQuery] PaginationRequest request)
        {
            var campaigns = await _campaignService.GetCampaignsWithOffers(request);
            return Ok(new ApiResponse<PaginationResponse<CampaignResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        /// <summary>
        /// Get all campaigns with offers by advertiser id
        /// </summary>
        /// <param name="id">Advertiser id</param>
        /// <param name="request">Pagination data</param>
        /// <returns></returns>
        [HttpGet("campaigns/advertisers/{id}/offers/")]
        [MapToApiVersion(1)]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaignsByAdvertiserWithOffers([FromQuery] PaginationRequest request, long id)
        {
            var campaigns = await _campaignService.GetCampaignsByAdvertisersWithOffers(request, id);
            return Ok(new ApiResponse<PaginationResponse<CampaignResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        //// GET: api/Campaigns/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Campaign>> GetCampaign(long id)
        //{
        //    var campaign = await _context.Campaigns.FindAsync(id);

        //    if (campaign == null)
        //    {
        //        return NotFound();
        //    }

        //    return campaign;
        //}

        //// PUT: api/Campaigns/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCampaign(long id, Campaign campaign)
        //{
        //    if (id != campaign.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(campaign).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CampaignExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Campaigns
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Campaign>> PostCampaign(Campaign campaign)
        //{
        //    _context.Campaigns.Add(campaign);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CampaignExists(campaign.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetCampaign", new { id = campaign.Id }, campaign);
        //}

        //// DELETE: api/Campaigns/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCampaign(long id)
        //{
        //    var campaign = await _context.Campaigns.FindAsync(id);
        //    if (campaign == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Campaigns.Remove(campaign);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CampaignExists(long id)
        //{
        //    return _context.Campaigns.Any(e => e.Id == id);
        //}
    }
}
