using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using EventCalendar.Core.Interfaces;
using EventCalendar.Core.Models;
using EventCalendar.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Core;
using OpenIddict.Models;

namespace EventCalendar
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CalendarDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<EventCalendarUser, IdentityRole>()
                .AddEntityFrameworkStores<CalendarDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(identityOptions =>
            {
                identityOptions.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                identityOptions.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                identityOptions.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<CalendarDbContext>();
                options.AddMvcBinders();
                options.EnableTokenEndpoint("/api/token");
                options.AllowPasswordFlow();
                options.DisableHttpsRequirement();
            });

            // Add framework services.
            services.AddMvc();

            services.AddScoped<IEventCalendarRepository, EventCalendarRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure identity.
            app.UseIdentity();
            app.UseOAuthValidation();
            app.UseOpenIddict();

            app.UseMvcWithDefaultRoute();
            app.UseWelcomePage();

            // Setup the application to use OpenIddict, create the DB if it doesn't exist, add a test user.
            ConfigureCalendarApp(app).GetAwaiter().GetResult();
        }

        private async Task ConfigureCalendarApp(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Check to make sure the database has been created from the migrations.
                var dbContext = app.ApplicationServices.GetRequiredService<CalendarDbContext>();
                await dbContext.Database.MigrateAsync();

                // Create the OpenIddict application if required.
                var manager =
                    scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                var cancellationToken = CancellationToken.None;

                if (await manager.FindByClientIdAsync("EventCalendar", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "EventCalendar",
                        DisplayName = "Event Calendar"
                    };

                    await manager.CreateAsync(application, "Secret-Key-For-Event-Calendar", cancellationToken);
                }

                // Add a test user to the database so I don't have to create an endpoint for making a new account.
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<EventCalendarUser>>();
                string userName = "test@test.com";

                if (await userManager.FindByNameAsync(userName) == null)
                {
                    await userManager.CreateAsync(new EventCalendarUser {UserName = userName, Email = userName},
                        "MyPassword_123");
                }
            }
        }
    }
}
