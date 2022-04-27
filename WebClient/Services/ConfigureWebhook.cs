using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WebClient.Services
{
	public class ConfigureWebhook : IHostedService
	{
		private readonly ILogger<ConfigureWebhook> _logger;
		private readonly IServiceProvider _services;
		private readonly BotConfiguration _botConfig;
		public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
							IServiceProvider serviceProvider,
							IConfiguration configuration)
		{
			_logger = logger;
			_services = serviceProvider;
			_botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using var scope = _services.CreateScope();
			var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

			var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
			_logger.LogInformation("Setting webhook: ", webhookAddress);
			await botClient.SetWebhookAsync(
				url: webhookAddress,
				allowedUpdates: Array.Empty<UpdateType>(),
				cancellationToken: cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
