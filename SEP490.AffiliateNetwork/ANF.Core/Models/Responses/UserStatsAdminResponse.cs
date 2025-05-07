namespace ANF.Core.Models.Responses
{
    public class UserStatsAdminResponse
    {
        public DateTime Date { get; set; }
        public int TotalUser { get; set; }

        // Field is not used currently
        public int? TotalActivedUser { get; set; }

        // Field is not used currently
        public int? TotalDeactivedUser { get; set; }
    }
}
