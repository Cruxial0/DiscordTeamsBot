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
using System.Diagnostics;

namespace CruxBot
{
    public interface IDependencyMap
    {
        void Add<T>(T obj) where T : class;
    }

    public class Program
    {
        //Initialize bot
        static void Main(string[] args)
        {
            Console.WriteLine("Bot starting...", Console.ForegroundColor = ConsoleColor.Yellow);
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        //Declare bot variables and file locations
        internal static CommandHandler handler;
        internal static DiscordSocketClient client;
        internal static CancellationTokenSource cancelSrc = new CancellationTokenSource();

        internal static string channelLocation = @"Config\channelId.txt";
        internal static string leaderLocation = @"Config\leaderRole.txt";
        internal static string teamLimitLocation = @"Config\teamLimit.txt";

        public async Task MainAsync()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            //Load permissions
            Permissions.Load();

            //Declare client
            client = new DiscordSocketClient();

            //Declare handler and add prefix
            handler = new CommandHandler(client, "-");

            //Start bot
            await client.SetGameAsync("team roles", "", ActivityType.Watching);
            await client.LoginAsync(TokenType.Bot, Secret.token);
            await client.StartAsync();

            //Check if config files exist
            if (!File.Exists(channelLocation)) File.Create(channelLocation);
            if (!File.Exists(leaderLocation)) File.Create(leaderLocation);
            if (!File.Exists(teamLimitLocation)) File.Create(teamLimitLocation);

            Console.WriteLine($"Bot Started Sucessfully! ({sw.ElapsedMilliseconds}ms)", Console.ForegroundColor = ConsoleColor.Yellow);

            sw.Stop();

            await Task.Delay(-1, cancelSrc.Token);
        }
    }
}
