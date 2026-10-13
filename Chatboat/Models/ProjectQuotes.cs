using Chatboat.Infra;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatboat.Models
{
    public class ProjectQuotes
    {
        [Key]
        public long ProjectQuoteId { get; set; }
        public long LeadId { get; set; }

        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? Budget { get; set; }
        public string? Timeline { get; set; }

        [NotMapped] public DateTime? CreatedAt { get; set; }
    }
}
