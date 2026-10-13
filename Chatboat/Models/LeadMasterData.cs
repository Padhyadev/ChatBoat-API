namespace Chatboat.Models
{
    public class LeadMasterData
    {
        public long LeadId { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }

        public string MasterService { get; set; }
        public string Service { get; set; }
        public string SubService { get; set; }

        public string Action { get; set; }
        public string FollowUp { get; set; }

        public string PlatformoptionsName { get; set; }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public decimal? Budget { get; set; }
        public string Timeline { get; set; }

        public string PreferredCallTime { get; set; }

        public string SupportDescription { get; set; }
        public string EnquiryName { get; set; }
    }
}
