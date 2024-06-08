var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("MyPolicy"); // Use CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();