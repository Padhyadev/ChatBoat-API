using Chatboat.Infra;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatboat.Models
{
    public class Leads : EntityBase
    {
        [Key]
        public long LeadId { get; set; }

        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Action { get; set; }
        public long? Service { get; set; }
        public long? SubService { get; set; }
        [Column("Platform Opation")]
        public long? PlatformOpation { get; set; }
        public long? ServiceMasterId { get; set; }
        public string? FollowUp { get; set; }
    }



}
