using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CleemyTest.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CleemyConnection"))) ;


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1",
                                new Microsoft.OpenApi.Models.OpenApiInfo
                                {
                                    Title = "CleemyTest API - V1",
                                    Version = "v1"
                                }
                             );

                            var filePath = Path.Combine(System.AppContext.BaseDirectory, "CleemyTest.xml");
                            c.IncludeXmlComments(filePath);
                        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
