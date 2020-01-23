using BotTools.Interfaces;
using CruxBot;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTeamsBot
{
    public static class Config
    {
        public static string channelLockId = File.ReadAllText(Program.channelLocation);
        public static string leaderRole = File.ReadAllText(Program.leaderLocation);
        public static string teamLimit = File.ReadAllText(Program.teamLimitLocation);
    }

    public class Prefrenes : IDiscordCommand
    {
        public string Name => "Prefrences";

        public string Help => "Shows sever prefrences";

        public string Syntax => "-prefrences";

        public string Permission => "default";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            if (parameters.Length != 2)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }
        }
    }
}
