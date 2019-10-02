using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using LisasTours.Application.Behaviors;
using LisasTours.Application.Commands.Companies;
using LisasTours.Application.Mapping;
using LisasTours.Application.Queries;
using LisasTours.Application.Validations;
using LisasTours.Data;
using LisasTours.Models.Identity;
using LisasTours.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LisasTours
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionString = GetConnectionString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            /*services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
                cfg.AddProfile(new UsersProfile(provider.GetRequiredService<UserManager<ApplicationUser>>()))
            ));*/

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation();

            //-------Validation--------
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IValidator<CreateCompanyCommand>, CreateCompanyCommandValidator>();

            services.AddSingleton<IExporter, ExcelExporter>();
            services.AddSingleton<CompanyFilterService>();
            services.AddScoped<ICompanyQueries, CompanyQueries>();
            services.AddScoped<IContactsQueries, ContactsQueries>();
            services.AddScoped<IUsersQueries, UsersQueries>();

            CreateRoles(services.BuildServiceProvider()).Wait();
        }

        private string GetConnectionString()
        {
            var connectionStringName = Configuration["SelectedConnectionString"] ?? "LocalDb";
            if (string.IsNullOrEmpty(Configuration.GetConnectionString(connectionStringName)))
            {
                throw new Exception("������� ������� ������ �����������");
            }
            var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString(connectionStringName));
            var password = Configuration[$"{connectionStringName}Password"];
            if (!string.IsNullOrEmpty(password))
            {
                builder.Password = Configuration[$"{connectionStringName}Password"];
            }
            var connectionString = builder.ConnectionString;
            return connectionString;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Добавляем роли
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleNames = typeof(RoleNames).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(_ => _.GetValue(null).ToString());
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Создаём супер-пользователя
            var superuser = new ApplicationUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };

            string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

            if (user == null)
            {
                var createSuperUser = await UserManager.CreateAsync(superuser, UserPassword);
                if (createSuperUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(superuser, RoleNames.Admin);
                }
            }
        }
    }
}
