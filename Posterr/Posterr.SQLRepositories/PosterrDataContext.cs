using Microsoft.EntityFrameworkCore;
using Posterr.CoreObjects.Entities;

namespace Posterr.SQLRepositories
{
    public class PosterrDataContext : DbContext
    {
        public PosterrDataContext() { }

        public PosterrDataContext(DbContextOptions<PosterrDataContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStats> UsersStats { get; set; }
        public DbSet<UserPostStats> UsersPostStats { get; set; }
        public DbSet<UserFollowings> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserStats>().ToTable("UsersStats");
            modelBuilder.Entity<UserPostStats>().ToTable("UsersPostsStats").HasKey(c => new { c.ReferenceDate, c.UserId });
            modelBuilder.Entity<UserFollowings>().ToTable("UsersFollowings").HasKey(c => new { c.UserId, c.FollowsThisId });
            modelBuilder.Entity<Post>().ToTable("Posts");
        }
    }
}
