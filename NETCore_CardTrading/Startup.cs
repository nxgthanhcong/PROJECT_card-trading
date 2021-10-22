using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore_CardTrading.Areas.Admin.Data;
using NETCore_CardTrading.Areas.Admin.Models;
using NETCore_CardTrading.Ultities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore_CardTrading
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký AppDbContext
            services.AddDbContext<AppDbContext>(options => {
                // Đọc chuỗi kết nối
                string connectstring = _config.GetConnectionString("DefaultConnection");
                // Sử dụng MS SQL Server
                options.UseSqlServer(connectstring);
            });
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/admin/login";
            });
            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;
            });

            services.AddAuthentication()
                .AddGoogle(googleOptions => {
                    googleOptions.ClientId = _config["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = _config["Authentication:Google:ClientSecret"];
                    googleOptions.CallbackPath = "/login-google";
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = _config["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = _config["Authentication:Microsoft:ClientSecret"];
                    microsoftOptions.CallbackPath = "/login-microsoft";
                })
                .AddFacebook(facebookOptions => {
                    facebookOptions.AppId = _config["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = _config["Authentication:Facebook:AppSecret"];
                    facebookOptions.CallbackPath = "/login-facebook";
                });
            ;

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IGmailService, GmailService>();
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
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Product}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "areaDefault",
                    areaName: "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                

                
                endpoints.MapRazorPages();
            });
        }
    }
}
