using Microsoft.EntityFrameworkCore;
using WebApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddCors(config =>
{
    config.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
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
app.UseRouting();
app.MapControllers();
app.UseCors("corsapp");
//app.MapControllerRoute(name: "default", pattern: "api/{controller=Home}/{action=Index}/{id?}");

app.Run();
