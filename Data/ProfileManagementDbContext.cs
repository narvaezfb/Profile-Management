using System;
using Microsoft.EntityFrameworkCore;
using Profile_Management.Models;
using System.Data;

namespace Profile_Management.Data
{
	public class ProfileManagementDbContext: DbContext
	{
        public ProfileManagementDbContext(DbContextOptions<ProfileManagementDbContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}

