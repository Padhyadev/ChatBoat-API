//namespace Chatboat.Models
//{
//    public class EnquiryType
//    {
//    }
//}



using Chatboat.Infra;
using System.ComponentModel.DataAnnotations;

namespace Chatboat.Models
{
    public class EnquiryType : EntityBase
    {
        [Key]
        public long EnquiryTypeId { get; set; }

        [Required]
        public string EnquiryTypeName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
