using System.ComponentModel.DataAnnotations;

namespace Chatboat.Models
{
    public class Platformoptions
    {

        [Key]
        public long PlatformoptionsId { get; set; }
        public string PlatformoptionsName { get; set; }
        public long SubServiceId { get; set; }
    }
}
