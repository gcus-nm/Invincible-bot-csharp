using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotConsole.Core
{
	public class DiscordBotClient
	{
		private readonly string BOT_TOKEN_FILEPATH = $"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}/../Resources/BotToken.txt";
		private const ulong BOT_CLIENT_ID = 1116820846678380614;

		public static DiscordSocketClient BotClient { get; private set; }
		public static CommandService BotCommandService { get; private set; }
		public static ServiceProvider BotServices { get; private set; }

		public static IMessageChannel ResentMessageChannel { get; private set; }
		public async Task MainAsync()
		{
			var config = new DiscordSocketConfig
			{
				GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
			};

			BotClient = new DiscordSocketClient(config);
			BotCommandService = new CommandService();
			BotServices = new ServiceCollection().BuildServiceProvider();

			BotClient.MessageReceived += OnRecievedMessage;

			try
			{
				string botToken = "";
				using (StreamReader read = new StreamReader(BOT_TOKEN_FILEPATH, Encoding.UTF8))
				{
					botToken = read.ReadLine();
				}

				if (string.IsNullOrEmpty(botToken))
				{
					Console.WriteLine("トークン取得失敗");
					throw new InvalidOperationException();
				}

				Console.WriteLine("Bot起動開始！");
				await BotCommandService.AddModulesAsync(Assembly.GetEntryAssembly(), BotServices);
				await BotClient.LoginAsync(TokenType.Bot, botToken);
				await BotClient.StartAsync();

				await Task.Delay(-1);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
			}
		}
		private static async Task OnRecievedMessage(SocketMessage message)
		{
			var userMessage = message as SocketUserMessage;

			await MessageToCommand(userMessage);
		}

		private static async Task MessageToCommand(SocketUserMessage userMessage)
		{
			if (!BotUtility.IsValidMessage(userMessage))
			{
				return;
			}

			int index = 0;

			if (!(userMessage.HasCharPrefix('#', ref index) || userMessage.HasMentionPrefix(BotClient.CurrentUser, ref index)))
			{
				return;
			}

			var context = new CommandContext(BotClient, userMessage);
			var command = await BotCommandService.ExecuteAsync(context, index, BotServices);

			ResentMessageChannel = context.Channel;

			if (!command.IsSuccess)
			{
				await context.Channel.SendMessageAsync(command.ErrorReason);
			}
		}
	}
}
