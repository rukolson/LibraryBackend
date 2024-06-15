// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using LibraryBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
	options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	options.SerializerSettings.ContractResolver = new DefaultContractResolver();
	options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
});

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Enable CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("MyPolicy",
		builder =>
		{
			builder.WithOrigins("http://localhost:3000")
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
	});
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
	   Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
	RequestPath = "/Photos"
});

app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.Use(async (context, next) =>
{
	context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
	context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
	context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
	Console.WriteLine("Handling request: " + context.Request.Path);
	await next.Invoke();
	Console.WriteLine("Finished handling request.");
});

app.MapControllers();

app.Run();
