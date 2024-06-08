using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.OpenApi.Models; // Dodane dla OpenApiInfo
using Swashbuckle.AspNetCore.SwaggerGen; // Dodane dla UseNewtonsoftSupport
using Swashbuckle.AspNetCore.Newtonsoft; // Dodane dla UseNewtonsoftSupport
using Newtonsoft.Json;

namespace project
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
			//Enable CORS
			services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy",
					builder =>
					{
						builder.WithOrigins("http://localhost:3000")
							   .AllowAnyHeader()
							   .AllowAnyMethod();
					});
			});

			//JSON Serializer
			services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
			});

			// Dodane dla Swaggera
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory()); //DO PÓŹNIEJSZEGO USUNIĘCIA

			//Enable CORS
			app.UseCors("MyPolicy");

			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
				context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
				context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
				Console.WriteLine("Handling request: " + context.Request.Path);
				await next.Invoke();
				Console.WriteLine("Finished handling request.");
			});

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

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
				   Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
				RequestPath = "/Photos"
			});

			// Dodane dla Swaggera
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
		}
	}
}