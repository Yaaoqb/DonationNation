using Amazon.S3;
using Amazon.XRay.Recorder.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DonationNation.Communication.Mail;
using DonationNation.Communication.SMS;
using DonationNation.Data;
using DonationNation.Data.Helpers;
using DonationNation.Data.Models;
using DonationNation.Filters;
using DonationNation.S3;
using Amazon.XRay.Recorder.Handlers.AwsSdk;


namespace DonationNation
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
            AWSXRayRecorder.InitializeInstance(Configuration);
            AWSSDKHandler.RegisterXRayForAllServices();

            //Add S3 and SNS Service
            services.AddSingleton<IS3Service, S3Service>();
            services.AddSingleton<ISNSService, SNSService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(UserActionFilter));
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
            });

            services.AddRazorPages();

            //Email
            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = Configuration["MailKit:SMTP:Address"];
                options.Host_Port = Convert.ToInt32(Configuration["MailKit:SMTP:Port"]);
                options.Host_Username = Configuration["MailKit:SMTP:Account"];
                options.Host_Password = Configuration["MailKit:SMTP:Password"];
                options.Sender_EMail = Configuration["MailKit:SMTP:SenderEmail"];
                options.Sender_Name = Configuration["MailKit:SMTP:SenderName"];
            });


            //AWS
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", Configuration["AWS:SessionToken"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            //Auto mapper for model mapping
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            EnsureAndSeedDb(serviceProvider, app);

            app.UseXRay("DonationNation");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void EnsureAndSeedDb(IServiceProvider serviceProvider, IApplicationBuilder app)
        {
            //Ensure database is created
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    dbContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }


            //initialize role manager and user manager
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //roles for the application
            string[] roles = new string[] { "Admin", "Donor" };
            //for each role
            foreach (string role in roles)
            {
                //check if role in database
                Task<bool> roleExists = RoleManager.RoleExistsAsync(role);
                roleExists.Wait();
                //if not in database, create role with role manager
                if (!roleExists.Result)
                {
                    Task<IdentityResult> roleResult = RoleManager.CreateAsync(new IdentityRole(role));
                    roleResult.Wait();
                }
            }


            var users = Configuration.GetSection("AppSettings:Users").Get<List<Users>>();
            foreach (var u in users)
            {
                //check if user exists in database
                Task<ApplicationUser> _user = UserManager.FindByEmailAsync(u.Email);
                _user.Wait();
                //if not, create user
                if (_user.Result == null)
                {
                    //Username and email is more than enough (But I like changing some of these hence declared)
                    var user = new ApplicationUser
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        UserName = u.Username,
                        PhoneNumber = u.Phone,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        LockoutEnabled = true,
                    };

                    Task<IdentityResult> createAdminUser = UserManager.CreateAsync(user, u.Password);
                    createAdminUser.Wait();

                    //if user creation succeeds
                    if (createAdminUser.Result.Succeeded)
                    {
                        //we tie the new user to the admin role
                        Task<IdentityResult> userRole = UserManager.AddToRoleAsync(user, u.Role);
                        userRole.Wait();
                    }
                }
            }
        }
    }
}
