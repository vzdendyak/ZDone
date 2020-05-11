using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ZDoneWebApi.BusinessLogic;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data;
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
            string connectionString = Configuration.GetConnectionString("Default");
            services.AddEntityFrameworkSqlServer().AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddCors();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin());
            });

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemBl, ItemBl>();

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

            services.AddScoped<IProjectsUsersRepository, ProjectsUsersRepository>();
            services.AddScoped<IProjectsUsersBl, ProjectsUsersBl>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentBl, CommentBl>();

            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "Sprint Controller", Version = "v2" });
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
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}