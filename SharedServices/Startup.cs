using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedServices.BL.Services;
using SharedServices.DAL;
using SharedServices.DAL.Interfaces;
using SharedServices.DAL.Repositories;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Services;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharedServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //i18n service
            services.AddLocalization(options => options.ResourcesPath = "Resources")
                    .AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

            //Framework services
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
            {
                services.AddDbContext<ApplicationContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("Default"))
                );
            }
            else
            {
                services.AddDbContext<ApplicationContext>();
            }

            services.AddControllersWithViews();

            services.AddIdentityCore<ApplicationUser>(o =>
                {
                    o.User.RequireUniqueEmail = false;
                    o.SignIn.RequireConfirmedAccount = true;
                    o.Password.RequireDigit = false;
                    o.Password.RequiredLength = 8;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = true;
                    o.Password.RequireLowercase = false;
                }
            ).AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<ApplicationContext>()
             .AddSignInManager()
             .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });

            // Add application services.
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDiscussionRepository, DiscussionRepository>();
            services.AddScoped<IServiceGroupRepository, ServiceGroupRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IGlobalInfoRepository, GlobalInfoRepository>();
            //Enterprise information
            services.AddTransient<IGlobalInfo, GlobalInfo>();

            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //i18n params
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("fr"),
                new CultureInfo("fr-FR"),
                new CultureInfo("en"),
                new CultureInfo("en-US")
            };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
