using Chat.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Message).IsRequired();
                entity.Property(x => x.Sender).IsRequired();
                entity.Property(x => x.Receiver).IsRequired();
            });

            builder.Entity<Room>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Members).IsRequired();
                entity.Property(x => x.RoomCreateor).IsRequired();
                entity.Property(x => x.RoomName).IsRequired();
            });
        }
    }
}