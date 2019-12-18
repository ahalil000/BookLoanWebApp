using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using BookLoan.Domain;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authorization;
using BookLoan.Authorization;

using Swashbuckle.AspNetCore;


namespace BookLoan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();

            // Inject ISwaggerProvider with defaulted settings
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "BookLoan API", Version = "v1" });
            });

            // Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BookReadAccess", policy =>
                {
                    policy.AddRequirements(
                        BookLoanOperations.Read);
                });
                options.AddPolicy("BookUpdateAccess", policy =>
                {
                    policy.AddRequirements(
                        BookLoanOperations.Update);
                });
                options.AddPolicy("BookCreateAccess", policy =>
                {
                    policy.AddRequirements(
                        BookLoanOperations.Create);
                });
                options.AddPolicy("BookLoanAccess", policy =>
                {
                    policy.AddRequirements(
                        BookLoanOperations.Loan);
                });
                options.AddPolicy("BookLoanAgeRestriction", policy =>
                {
                    policy.AddRequirements(
                        new MinimumAgeRequirement(18));
                });
            });

            // Authorization handlers.
            services.AddSingleton<IAuthorizationHandler, BookReadAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, BookUpdateAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, BookCreateAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, BookLoanAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, BookLoanAgeRestrictionHandler>();

            //services.AddOptions();
            services.Configure<AppConfiguration>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePagesWithRedirects("/Common/StatusCode?code={0}");
            }
            else
            {
                app.UseExceptionHandler("/Common/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLoan API");
                    c.RoutePrefix = string.Empty;
                }
            );

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //context.Database.EnsureDeleted(); // recreate DB -- for demo we comment out/remove this line.
                context.Database.EnsureCreated(); // create database if not already created.

                //var config = app.ApplicationServices.GetService<AppConfiguration>();
                Initialize(context);
            }
        }


        public void Initialize(ApplicationDbContext context)
        {
            // Do any initialization code here for the DB. 
            // Can include populate lookups, data based configurations etc.
            SeedData seed_data = new SeedData(context);
            seed_data.GenerateBooks();

            var appOptions = new AppConfiguration();
            Configuration.GetSection("AppSettings").Bind(appOptions);

            SeedAccounts seed_acc = new SeedAccounts(context, appOptions);
            var task = Task.Run(async() => await seed_acc.GenerateUserAccounts());
            var result = task.Wait(5000);
        }
    }
}