﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Seeds;
using SharedServices.Mutual;
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
                optionsBuilder.UseSqlServer("Server=AMBROISE-PC,1433;Database=BetweenUsDBProd;User Id=BetweenUs; Password=Ambroise0;Trusted_Connection=True;MultipleActiveResultSets=true");
                //optionsBuilder.UseSqlite("Data Source=BetweenUsDB.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Many to many between Service and ApplicationUser
            builder.Entity<ApplicationUserServices>().HasKey(s => new { s.ServiceId, s.ApplicationUserId });

            builder.Entity<ApplicationUserServices>()
                   .HasOne(x => x.Service)
                   .WithMany(x => x.UserServices)
                   .HasForeignKey(x => x.ServiceId);

            builder.Entity<ApplicationUserServices>()
                   .HasOne(x => x.User)
                   .WithMany(x => x.UserServices)
                   .HasForeignKey(x => x.ApplicationUserId);

            //Many to many between RequestMulticast and ApplicationUser
            builder.Entity<ResponseMulticastRequest>().HasKey(r => new { r.RequestMulticastId, r.ApplicationUserId });

            builder.Entity<ResponseMulticastRequest>()
                   .HasOne(x => x.RequestMulticast)
                   .WithMany(x => x.Responses)
                   .HasForeignKey(x => x.RequestMulticastId);

            builder.Entity<ResponseMulticastRequest>()
                   .HasOne(x => x.Responder)
                   .WithMany(x => x.Responses)
                   .HasForeignKey(x => x.ApplicationUserId);
            
            //Data seeds
            builder.Entity<GlobalInfo>().HasData(GlobalInfoSeed.GlobalInfo());
            builder.Entity<NotificationType>().HasData(NotificationTypeSeed.SeedTypes());
            builder.Entity<ServiceGroup>().HasData(ServiceGategorySeed.GetCategories());
            builder.Entity<Service>().HasData(SeedServices.GetServices());

            base.OnModelCreating(builder);
        }

        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestMulticast> RequestMulticasts { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FaqQuestion> FaqQuestions { get; set; }
        public DbSet<FaqResponse> FaqResponses { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<GlobalInfo> Infos { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
    }
}