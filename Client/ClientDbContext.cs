using DBCore.Model;
using DBCorev2;

namespace Client
{
    public class ClientDbContext : InMemoryDB
    {
        public InMemoryTable<Event> Events { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id)
                .HasIndex(e => e.SecondId)
                //.HasIndex(e => e.ThirdId).IsUnique();
                .HasIndex(e => new {e.SecondId, e.ThirdId});
            //.HasIndex(e => e.Name);
        }
    }
}