using Microsoft.EntityFrameworkCore;
using MusicApp.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MusicApp.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor accepts options injected via dependency injection.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        // DbSets for each entity.
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }
        public DbSet<WishlistSong> WishlistSongs { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for PlaylistSong (join table)
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            // Configure composite key for FavoriteSong
            modelBuilder.Entity<FavoriteSong>()
                .HasKey(fs => new { fs.AppUserId, fs.SongId });

            // Configure composite key for WishlistSong
            modelBuilder.Entity<WishlistSong>()
                .HasKey(ws => new { ws.AppUserId, ws.SongId });

            // Configure a unique index on Username and Email for AppUser
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure composite key for Friendship
            modelBuilder.Entity<Friendship>()
                .HasKey(f => new { f.RequesterId, f.AddresseeId });

            // Configure relationship for friend requests sent by a user
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship for friend requests received by a user
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Addressee)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
