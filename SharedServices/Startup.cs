using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing.Constraints;
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
            //Trying to resolve session error for sign in manager
            //services.AddSession();
            services.AddSignalR();

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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.Cookie.Name = "YourAppCookieName";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            //    options.LoginPath = "/Account/Login";
            //    // ReturnUrlParameter requires 
            //    //using Microsoft.AspNetCore.Authentication.Cookies;
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});

            // Add application services.
            services.AddTransient<ICompositeViewEngine, CompositeViewEngine>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDiscussionRepository, DiscussionRepository>();
            services.AddScoped<IServiceGroupRepository, ServiceGroupRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IGlobalInfoRepository, GlobalInfoRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            //Enterprise information
            services.AddTransient<IGlobalInfo, GlobalInfo>();

            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<IBroadcastEmailSender, BroadcastEmailSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
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

            app.UseSession();

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
            localizationOptions.RequestCultureProviders.Insert(0, new UrlRequestCultureProvider());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "cultureRoute",
                    pattern: "{culture=fr}/{controller=Home}/{action=Index}/{id?}",
                    constraints: new
                    {
                        culture = new RegexRouteConstraint("^[a-z]{2}(?:-[A-Z]{2})?$")
                    });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<SignalRHub>("/params");
            });
        }
    }
}