using System.Collections.Generic;
using System.Linq;

namespace DiscordBotConsole.Minecraft
{
	/// <summary>
	/// サーバーごとの起動等に必要な情報がまとまってる！
	/// </summary>
	public class MinecraftServerData
	{
		public readonly static MinecraftServerData[] MINECRAFT_SERVERS = new MinecraftServerData[]
		{
			new MinecraftServerData("1.12.2Mohist",			"1.12.2 匠サーバー",			8,		"takumi"),
			new MinecraftServerData("1.12.2Kumada",			"1.12.2 Modサーバー",			8,		"1"),
			new MinecraftServerData("1.12.2SkyFactory4",	"1.12.2 SkyFactory4",			8,		"sky", "skyfactory" ),
			new MinecraftServerData("1.18.1P",				"1.18.1 軽量バニラサーバー",	17,		"1.18.1", "1.18.1P" ),
			new MinecraftServerData("1.19",					"1.19 バニラサーバー",			17,		"1.19.0", "1.19"),
			new MinecraftServerData("1.19.3",				"1.19.3 バニラサーバー",		17,		"1.19.3"),
			new MinecraftServerData("1.20.0Creative",		"1.20.0 クリエイティブサーバー",17,		"1.20.0creative", "1.20creative", "create"),
			new MinecraftServerData("1.20.0",				"1.20.0 バニラサーバー",		17,		"1.20.0", "1.20"),
		};

		public MinecraftServerData(string serverName, string serverFriendlyName, int javaVersion, params string[] buildServerTexts)
		{
			ServerName = serverName;
			ServerFriendlyName = serverFriendlyName;
			JavaVersion = javaVersion;
			BuildServerTexts = buildServerTexts.Union(new string[] { serverName, serverFriendlyName }.Distinct()).ToArray();
		}
		public MinecraftServerData(string serverName, int javaVersion, params string[] buildServerTexts) : this(serverName, serverName, javaVersion, buildServerTexts)
		{
		}

		/// <summary>
		/// サーバー名
		/// </summary>
		public string ServerName { get; private set; }

		/// <summary>
		/// わかりやすいサーバー名
		/// </summary>
		public string ServerFriendlyName { get; private set; }

		/// <summary>
		/// コマンドでサーバーを起動するときに利用できる名前
		/// </summary>
		public string[] BuildServerTexts { get; private set; }

		/// <summary>
		/// サーバー起動時に利用するJavaのバージョン
		/// </summary>
		public int JavaVersion { get; private set; }
	}
}
