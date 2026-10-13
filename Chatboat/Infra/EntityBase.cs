using System.ComponentModel.DataAnnotations.Schema;

namespace Chatboat.Infra
{
    public class EntityBase
    {
        [NotMapped]
        public long? CreatedBy { get; set; }
        [NotMapped]

        public DateTime? CreatedDate { get; set; }
        [NotMapped]

        public long? LastModifiedBy { get; set; }

        [NotMapped]

        public DateTime? LastModifiedDate { get; set; }

    }
}
