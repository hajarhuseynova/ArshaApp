using Arsha.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.Context
{
    public class ArshaDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<SocialMedia> SocialMedias { get; set; }

        public DbSet<TeamMembers> TeamMembers { get; set; }

        public DbSet<Item> Items { get; set; }
        public ArshaDbContext(DbContextOptions<ArshaDbContext> options) : base(options)
        {

        }
    }
}
