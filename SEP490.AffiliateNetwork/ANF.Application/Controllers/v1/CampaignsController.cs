using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Models.Entities;
using ANF.Infrastructure;
using ANF.Core.Services;

namespace ANF.Application.Controllers.v1
{
    public class CampaignsController(ICampaignService campaignService) : BaseApiController
    {
        private readonly ICampaignService _campaignService = campaignService;

        // GET: api/Campaigns
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns()
        //{
        //    return await _context.Campaigns.ToListAsync();
        //}

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
