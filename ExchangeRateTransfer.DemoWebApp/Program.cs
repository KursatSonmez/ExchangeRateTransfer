using ExchangeRateTransfer.DemoWebApp.DataContext;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Services;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DemoWebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();

			ConfigureServices(builder);

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

			app.UseRouting();

			app.MapBlazorHub();
			app.MapFallbackToPage("/_Host");

			using (var scope = app.Services.CreateScope())
			{
				using var seeder = new DataSeeder(scope.ServiceProvider.GetRequiredService<DemoWebAppDbContext>());

				seeder.Seed();
			}

			app.Run();
		}

		private static void ConfigureServices(WebApplicationBuilder builder)
		{
			var services = builder.Services;

			IConfiguration configuration = builder.Configuration;

			bool isDevelopment = builder.Environment.IsDevelopment();

			string connectionStr = configuration.GetConnectionString("DbContext");

			services.AddDbContext<DemoWebAppDbContext>(x =>
			{
				x.UseSqlServer(connectionStr);

				if (isDevelopment)
					x.EnableSensitiveDataLogging();
			});

			var workingHour = configuration["ExchangeRateTransfer:WorkingHour"];

			services.EnableExchangeRateTransfer(x =>
			{
				x.TimerPeriod = TimeSpan.FromMinutes(10);

				if (!string.IsNullOrWhiteSpace(workingHour))
					x.WorkingHour = workingHour;

				x.SkipWorkingHour = isDevelopment;

				return x;
			}); // this is required

			services.AddScoped<IExchangeRateService, ExchangeRateService>();
			services.AddScoped<ILiraExchangeRateTransferService, LiraExchangeRateTransferService>();
		}
	}
}