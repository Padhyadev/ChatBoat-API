using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chatboat.Models
{
    public class MasterServices
    {
        [Key]
        public long MasterService_Id { get; set; }

        public string MasterService_Name { get; set; }

        public bool Status { get; set; }

        public int Display_Seq_No { get; set; }

    }
}
