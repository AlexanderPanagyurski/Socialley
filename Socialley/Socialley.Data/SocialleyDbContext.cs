using Microsoft.EntityFrameworkCore;
using Socialley.Common;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data
{
    public class SocialleyDbContext : DbContext
    {
        public SocialleyDbContext( DbContextOptions options)
            : base(options)
        {
        }

        protected SocialleyDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        public DbSet<BlockedPost> BlockedPosts { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<FavouritePost> FavouritePosts { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostLike> PostsLikes { get; set; }

        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<RecommendedFriend> RecommendedFriends { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UsersGroups { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        // TODO: Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
