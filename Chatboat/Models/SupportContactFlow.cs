using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatboat.Models
{
    [Table("Support_ContactFlow")]
    public class SupportContactFlow
    {
        [Key]
        public long Id { get; set; }
        public long EnquiryTypeId { get; set; }
        public long LeadId { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
    }
}
