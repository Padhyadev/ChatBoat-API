using System.ComponentModel.DataAnnotations;

namespace Chatboat.Models
{
    public class TalkToExpert
    {
        [Key]
        public long Id { get; set; }

        public string Email { get; set; }

        public string PreferredCallTime { get; set; }

        public long? LeadId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
