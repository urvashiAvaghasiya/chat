using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace chat.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TblChat>()
                .HasOne<TblUser>(s => s.SenderUser)
                .WithMany(g => g.ChatSenderUser)
                .HasForeignKey(s => s.SenderId)
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TblChat>()
                .HasOne<TblUser>(s => s.ReceiverUser)
                .WithMany(g => g.ChatReciverUser)
                .HasForeignKey(s => s.ReceiverId)
    .OnDelete(DeleteBehavior.Restrict);
        }




        public DbSet<TblUser> TblUser { get; set; }
        public DbSet<TblChat> TblChat { get; set; }

    }
}
