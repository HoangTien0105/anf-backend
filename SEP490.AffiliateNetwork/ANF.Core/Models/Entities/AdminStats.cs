using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Statistics for admin dashboard.
    /// </summary>
    public class AdminStats : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("total_user")]
        public int TotalUser { get; set; }

        [Column("total_campaign")]
        public int TotalCampaign { get; set; }

        [Column("total_rejected_campaign")]
        public int TotalRejectedCampaign { get; set; }

        [Column("total_approved_campaign")]
        public int TotalApprovedCampaign { get; set; }

        [Column("total_ticket")]
        public int TotalTicket { get; set; }

        [Column("total_resolved_ticket")]
        public int TotalResolvedTicket { get; set; }

        [Column("total_pending_ticket")]
        public int TotalPendingTicket { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
