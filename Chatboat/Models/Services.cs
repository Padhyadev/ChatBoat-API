using Chatboat.Infra;

namespace Chatboat.Models
{
    public class Services : EntityBase
    {
        public long Service_Id { get; set; }
        public string Service_Name { get; set; }
        public bool Status { get; set; }
        public int Display_Seq_No { get; set; }
    }
}
