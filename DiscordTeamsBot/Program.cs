using BotTools;
using BotTools.Handlers;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Threading;
using DiscordTeamsBot;
using System.IO;
using DiscordTeamsBot.Commands;

namespace CruxBot
{
    public interface IDependencyMap
    {
        void Add<T>(T obj) where T : class;
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bot starting...", Console.ForegroundColor = ConsoleColor.Yellow);
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        internal static CommandHandler handler;
        internal static DiscordSocketClient client;
        internal static CancellationTokenSource cancelSrc = new CancellationTokenSource();

        internal static string channelLocation = @"Config\channelId.txt";
        internal static string leaderLocation = @"Config\leaderRole.txt";
        internal static string teamLimitLocation = @"Config\teamLimit.txt";

        public async Task MainAsync()
        {
            Permissions.Load();

            client = new DiscordSocketClient();

            handler = new CommandHandler(client, "-");

            await client.SetGameAsync("team roles", "", ActivityType.Watching);
            await client.LoginAsync(TokenType.Bot, Secret.token);
            await client.StartAsync();

            if (!File.Exists(channelLocation)) File.Create(channelLocation);
            if (!File.Exists(leaderLocation)) File.Create(leaderLocation);
            if (!File.Exists(teamLimitLocation)) File.Create(teamLimitLocation);

            Console.WriteLine("Bot Started Sucessfully!", Console.ForegroundColor = ConsoleColor.Green);

            await Task.Delay(-1, cancelSrc.Token);
        }
    }
}
