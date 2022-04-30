using DependencyCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using WebClient.Services;

namespace WebClient
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			BotConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
		}

		public IConfiguration Configuration { get; }
		private BotConfiguration BotConfig { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHostedService<ConfigureWebhook>();

			ITelegramBotClient botClient = null!;

			services.AddHttpClient("tgwebhook")
				.AddTypedClient(httpClient => 
				{
					botClient = new TelegramBotClient(BotConfig.BotToken, httpClient);
					AutofacContainer.InitContainer(Configuration, botClient);
					return botClient;
				});

			services
				.AddScoped<HandleUpdateService>()
				.AddControllers()
				.AddNewtonsoftJson();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseCors();


			app.UseEndpoints(endpoints =>
			{
				var token = BotConfig.BotToken;
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllerRoute(
					name: "tgwebhook",
					pattern: $"bot/{token}",
					new { controller = "Webhook", action = "Post" });
				endpoints.MapControllers();
			});
		}
	}
}
