using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer.Database
{
    public class EtcsDbContext : DbContext
    {
        public EtcsDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Crossing> Crossings { get; set; }
        public DbSet<CrossingTrack> CrossingTracks { get; set; }
        public DbSet<RailwaySignal> TrackSignals { get; set; }
        public DbSet<SwitchRoute> TrackSwitches { get; set; }
        public DbSet<SwitchDirection> SwitchDirections { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<RailroadSign> Signs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Track>().ToTable(nameof(Track));
            modelBuilder.Entity<Switch>().ToTable(nameof(Switch));
            modelBuilder.Entity<SwitchingTrack>().ToTable(nameof(SwitchingTrack));

            modelBuilder.Entity<TrackageElement>()
                .HasOne(te => te.RightSideElement)
                .WithMany()
                .HasForeignKey(te => te.RightSideElementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrackageElement>()
                .HasOne(te => te.LeftSideElement)
                .WithMany()
                .HasForeignKey(te => te.LeftSideElementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SwitchRoute>()
                .HasOne(sr => sr.Switch)
                .WithMany()
                .HasForeignKey(sr => sr.SwitchId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SwitchRoute>()
                .HasOne(sr => sr.TrackFrom)
                .WithMany()
                .HasForeignKey(sr => sr.TrackFromId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SwitchRoute>()
                .HasOne(sr => sr.TrackTo)
                .WithMany()
                .HasForeignKey(sr => sr.TrackToId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
