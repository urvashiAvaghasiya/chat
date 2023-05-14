using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Data
{
    public class TblChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string? MediaType { get; set; }
        public string? MediaUrl { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }


        [ForeignKey("SenderId")]
        public virtual TblUser SenderUser { get; set; } = null!;

        [ForeignKey("ReceiverId")]
        public virtual TblUser ReceiverUser { get; set; } = null!;
    }
}
