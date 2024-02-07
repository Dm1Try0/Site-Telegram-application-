using Microsoft.EntityFrameworkCore;
using MProj6.DataAccess;
using MProj6.DataAccess.Contracts;
using MProj6.DataAccess.Internals;

var builder = WebApplication.CreateBuilder(args);



string connection = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connection))
	 .AddTransient<IIndexPageItemsRepository, IndexPageItemsRepository>()
	 .AddTransient<ITitleStoriesRepository, TitleStoriesRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=All}/{id?}");

app.Run();
