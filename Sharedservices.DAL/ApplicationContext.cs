﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using System;

namespace SharedServices.DAL
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationContext(){}

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            if (options is null)
            {
                throw new ArgumentNullException($"DbContext options cannot be null. {nameof(options)}");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null)
            {
                throw new ArgumentNullException($"DbContextOptionsBuilder object cannot be null. {nameof(optionsBuilder)}");
            }

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SharedServicesDB;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
    }
}