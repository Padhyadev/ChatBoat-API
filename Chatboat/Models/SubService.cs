using System.ComponentModel.DataAnnotations;

namespace Chatboat.Models
{
    public class SubService
    {
        [Key]
        public long SubServiceId { get; set; }
        public string SubServiceName { get; set; }
        public long ServiceId { get; set; }
    }
}
