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

            if (parameters.Length != 0)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }

            ulong roleID = Convert.ToUInt64(Config.leaderRole);

            SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;

            var role = server.GetRole(roleID);

            eb.AddField("Bot channel", $"<#{Config.channelLockId}>");
            eb.AddField("Leader role", $"{role.Mention}");
            eb.AddField("Team limit", $"{Config.teamLimit}");

            eb.Color = Color.Teal;

            efb.Text = DateTime.Now.ToString();
            efb.IconUrl = msg.Author.GetAvatarUrl();

            eb.WithFooter(efb);

            var embed = eb.Build();

            await msg.Channel.SendMessageAsync("", embed: embed);
        }
    }

    class ClearConfig : IDiscordCommand
    {
        public string Name => "clearConfig";

        public string Help => "clears the config file";

        public string Syntax => "-clearConfig";

        public string Permission => "admin";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            if(parameters.Length != 0)
            {
                await msg.Channel.SendMessageAsync($"**Wrong usage!, `{Syntax}`**");
            }

            File.WriteAllText(Config.channelLockId, "");
            File.WriteAllText(Config.teamLimit, "");
            File.WriteAllText(Config.leaderRole, "");

            await msg.Channel.SendMessageAsync("**Config file was cleared!**");
        }
    }
}
