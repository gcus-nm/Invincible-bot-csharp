using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DiscordBotConsole
{
	public static class BotUtility
	{
		/// <summary>
		/// メッセージが有効かどうか
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool IsValidMessage(SocketMessage message)
		{
			if (message == null)
			{
				return false;
			}

			if (message.Author.IsBot)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 実行環境を返す
		/// </summary>
		/// <returns></returns>
		public static OSPlatform GetServerOS()
		{
			var servers = new OSPlatform[3]
			{
				OSPlatform.Windows,
				OSPlatform.OSX,
				OSPlatform.Linux
			};

			for (int i = 0; i < servers.Length; ++i)
			{
				if (RuntimeInformation.IsOSPlatform(servers[i]))
				{
					return servers[i];
				}
			}

			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// 実行環境と一致するバージョンのペアの値を返す
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="osValuePair"></param>
		/// <returns></returns>
		public static T GetValueFromOS<T>(KeyValuePair<OSPlatform, T>[] osValuePair)
		{
			var value = osValuePair.FirstOrDefault(pair => RuntimeInformation.IsOSPlatform(pair.Key));

			return value.Equals(default(KeyValuePair<OSPlatform, T>)) ? default : value.Value;
		}

		/// <summary>
		/// 実行環境と一致するバージョンのペアのActionを実行する
		/// </summary>
		/// <param name="osActionPair"></param>
		public static void DoActionFromOS(KeyValuePair<OSPlatform, Action>[] osActionPair)
		{
			Action act = GetValueFromOS(osActionPair);
			act?.Invoke();
		}

		/// <summary>
		/// 環境によって自動的にShell実行を切り替える
		/// </summary>
		/// <param name="shellFilePath"></param>
		/// <returns></returns>
		public static Process ShellStartForEnvironment(string shellFilePath)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return ShellStartForWindows(shellFilePath);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return ShellStartForMac(shellFilePath);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// WindowsでShell実行
		/// </summary>
		/// <param name="shellFilePath"></param>
		/// <returns></returns>
		public static Process ShellStartForWindows(string command)
		{
			var terminalPath = "cmd.exe";

			var shell = new ProcessStartInfo(terminalPath, $"/k {command}");

			return Process.Start(shell);
		}

		/// <summary>
		/// macOSでShell実行
		/// </summary>
		/// <param name="shellFilePath"></param>
		/// <returns></returns>
		public static Process ShellStartForMac(string command)
		{
			var shell = new ProcessStartInfo()
			{
				FileName = "osascript",
				Arguments = $"-e 'tell application \"Terminal\" to activate' -e 'tell application \"Terminal\" to do script \"{command}\"'",

				UseShellExecute = true,
				CreateNoWindow = false,
				Verb = "runas",
				RedirectStandardOutput = false,
				RedirectStandardInput = false,
			};

			return Process.Start(shell);
		}

		/// <summary>
		/// 条件がTrueの間、待ち続ける
		/// </summary>
		/// <param name="status">whileループに利用する条件</param>
		/// <param name="timeout">タイムアウト時間</param>
		/// <param name="checkInterval">条件式の確認間隔</param>
		/// <returns></returns>
		public static async Task WaitWhile(Func<bool> status, int timeout = 30000, int checkInterval = 100)
		{
			int loopLimit = timeout / checkInterval;
			for (int i = 0; i < loopLimit; ++i)
			{
				if (!status.Invoke())
				{
					return;
				}

				await Task.Delay(100);
			}

			throw new TimeoutException();
		}
		
		/// <summary>
		/// 条件がTrueの間、待ち続ける
		/// </summary>
		/// <param name="status">whileループに利用する条件</param>
		/// <param name="timeout">タイムアウト時間</param>
		/// <param name="checkInterval">条件式の確認間隔</param>
		/// <returns></returns>
		public static async Task WaitWhile(Task<bool> status, int timeout = 60000, int checkInterval = 100)
		{
			int loopLimit = timeout / checkInterval;
			for (int i = 0; i < loopLimit; ++i)
			{
				if (await status)
				{
					return;
				}

				await Task.Delay(100);
			}

			throw new TimeoutException();
		}
	}
}
