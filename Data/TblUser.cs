using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Data
{
    public class TblUser
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<TblChat> ChatSenderUser { get; set; } = new List<TblChat>();
 
        public virtual ICollection<TblChat> ChatReciverUser { get; set; } = new List<TblChat>();

    }
}
