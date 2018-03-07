using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToDoWebApiApp.Models;

namespace ToDoWebApiApp
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
			services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("ToDoListDb"));

			//services.AddDbContext<TodoDbContext>(opt =>
			//	opt.UseSqlServer(Configuration.GetConnectionString("ToDoDatabaseConnectionString")));

			/*
			 options.UseSqlServer("")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<GkRTLSContext>()
                .AddDefaultTokenProviders();
			 */
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}

			if (env.IsProduction() || env.IsStaging())
			{
				app.UseExceptionHandler("/Error");
			}

			//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?tabs=aspnetcore2x
			app.UseStaticFiles();

			//app.UseMvc(routes =>
			//{
			//	routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			//});
			//app.UseMvc();
			app.UseMvcWithDefaultRoute();
			/*
			 * //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing
			 * 
			
			*/
		}
	}
}
