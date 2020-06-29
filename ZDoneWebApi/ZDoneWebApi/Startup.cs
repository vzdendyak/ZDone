using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ZDoneWebApi.BusinessLogic;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Middleware;
using ZDoneWebApi.Repositories;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi
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
            string connectionString = Configuration.GetConnectionString("Azure");
            //if (env.IsProduction())
            //{
            //    connectionString = Configuration.GetConnectionString("Azure");
            //}
            services.AddEntityFrameworkSqlServer().AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<User, IdentityRole>(set =>
            {
                set.Password = new PasswordOptions()
                {
                    RequireNonAlphanumeric = false,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequiredLength = 8
                };
                set.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            var tokenParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtIssuer"],
                ValidAudience = Configuration["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtKey"])),
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenParameters;
                options.RequireHttpsMetadata = true;
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddCors();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin());
            });
            services.AddSingleton(tokenParameters);
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemBl, ItemBl>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectBl, ProjectBl>();

            services.AddScoped<IListRepository, ListRepository>();
            services.AddScoped<IListBl, ListBl>();

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IFolderBl, FolderBl>();

            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IStatusBl, StatusBl>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserBl, UserBl>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountBl, AccountBl>();

            services.AddScoped<IAuthBl, AuthBl>();

            services.AddScoped<IProjectsUsersRepository, ProjectsUsersRepository>();
            services.AddScoped<IProjectsUsersBl, ProjectsUsersBl>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentBl, CommentBl>();

            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "Sprint Controller", Version = "v2" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                     {"Bearer", new string[0] }
                };
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             },
                             Scheme = "oauth2",
                             Name = "Bearer",
                             In = ParameterLocation.Header,
                         },
                         new List<string>()
                     }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder.WithOrigins("https://localhost:4201").AllowCredentials().AllowAnyMethod().AllowAnyHeader());
            app.UseMiddleware<AuthMiddleware>();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}