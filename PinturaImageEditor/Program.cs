using Microsoft.EntityFrameworkCore;
using PinturaImageEditor.Components;
using PinturaImageEditor.Data;
using PinturaImageEditor.Services;
using Radzen;

namespace PinturaImageEditor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();


			builder.Services.AddDbContextPool<CoreDbContext>(options =>
				options.UseInMemoryDatabase("PinturaDB"));


			builder.Services.AddRadzenComponents();
			builder.Services.AddScoped<ArchiveService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			
			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			app.Run();
		}
	}
}
