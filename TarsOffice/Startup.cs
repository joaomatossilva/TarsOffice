using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TarsOffice.Data;
using TarsOffice.DayFeatures;
using TarsOffice.DayFeatures.Abstractions;
using TarsOffice.Services.Abstractions;

namespace TarsOffice
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages(opt =>
            {
                opt.Conventions.AuthorizeFolder("/");
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "tarsatoffice_2111";
            });

            services.AddAuthentication(options =>
                {
                    //options.
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                    options.SaveTokens = true;
                    options.Scope.Add("profile");
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        List<AuthenticationToken> tokens = context.Properties.GetTokens().ToList();

                        tokens.Add(new AuthenticationToken()
                        {
                            Name = "TicketCreated",
                            Value = DateTime.UtcNow.ToString()
                        });

                        context.Properties.StoreTokens(tokens);

                        return Task.CompletedTask;
                    };

                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    options.ClaimActions.MapJsonKey("urn:google:name", "name", "string");
                });

            services.AddSingleton<IDayFeature, PastelNataDayFeature>();
            services.AddSingleton<IDayFeature, CroissantDayFeature>();
            services.AddSingleton<IDayFeature, CakeDayFeature>();
            services.AddSingleton<IDayFeature, CupCakeDayFeature>();

            services.AddScoped<ISiteService, SiteService>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
