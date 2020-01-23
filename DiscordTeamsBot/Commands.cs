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

namespace DiscordTeamsBot.Commands
{
    public class Test : IDiscordCommand
    {
        public string Name => "test";

        public string Help => "test stuff";

        public string Syntax => "-test";

        public string Permission => "default";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            await msg.Channel.SendMessageAsync($"Hey {msg.Author.Mention}! This command is indeed working just fine!");
        }
    }

    public class SetChannel : IDiscordCommand
    {
        public string Name => "SetChannel";

        public string Help => "Sets a channel for bot operation.";

        public string Syntax => "-setChannel {text channel}";

        public string Permission => "default";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            if (msg.Channel.Id != Convert.ToUInt64(File.ReadAllText(Program.channelLocation)))
            {
                string tcID = File.ReadAllText(Program.channelLocation);

                eb.AddField("Locked!", $"**This bot can only be used in <#{tcID}>**");
                eb.Color = Color.Red;

                var embedLocked = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLocked);
                return;
            }

            if (parameters.Length != 1)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }

            string input = parameters[0];

            string i1 = input.Replace("#", "");
            string i2 = i1.Replace("<", "");
            string id = i2.Replace(">", "");

            ulong channelId = Convert.ToUInt64(id);

            string targetId = msg.MentionedUsers.Count == 1 ? msg.MentionedUsers.First().Id.ToString() : parameters[0];
            SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;
            SocketGuildUser target = server.Users.FirstOrDefault(x => x.Id.ToString() == targetId);

            var channel = server.GetChannel(channelId);

            File.WriteAllText(Program.channelLocation, String.Empty);
            File.WriteAllText(Program.channelLocation, id);

            var fileContent = File.ReadAllText(Program.channelLocation);

            await msg.Channel.SendMessageAsync($"**This bot has been locked to:** <#{fileContent}>");
        }
    }

    class SetLeaderRole : IDiscordCommand
    {
        public string Name => "setLeaderRole";

        public string Help => "Sets the team leader role.";

        public string Syntax => "-setLeaderRole {@Role}";

        public string Permission => "admin";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            string s1 = parameters[0].Replace("<", "");
            string s2 = s1.Replace(">", "");
            string s3 = s2.Replace("!", "");
            string s4 = s3.Replace("@", "");
            string final = s4.Replace("&", "");

            string targetId = msg.MentionedUsers.Count == 1 ? msg.MentionedUsers.First().Id.ToString() : parameters[0];
            SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;
            SocketGuildUser target = server.Users.FirstOrDefault(x => x.Id.ToString() == targetId);

            var role = server.GetRole(Convert.ToUInt64(final));

            string FileInput = Config.leaderRole;

            if(final == FileInput)
            {
                eb.AddField("Warning!", $"**The leader role is already set to {role.Mention}**");
                eb.Color = Color.Red;

                efb.IconUrl = msg.Author.GetAvatarUrl();
                efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embed = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embed);

                return;
            }

            if (msg.Channel.Id != Convert.ToUInt64(File.ReadAllText(Program.channelLocation)))
            {
                string tcID = File.ReadAllText(Program.channelLocation);

                eb.AddField("Locked!", $"**This bot can only be used in <#{tcID}>**");
                eb.Color = Color.Red;

                var embedLocked = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLocked);
                return;
            }

            if (parameters.Length != 1)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }

            File.WriteAllText(Program.leaderLocation, string.Empty);
            File.WriteAllText(Program.leaderLocation, final);

            await msg.Channel.SendMessageAsync($"**{role.Mention} has been set as the leader role!**");
        }
    }

    class SetTeamLimit : IDiscordCommand
    {
        public string Name => "setTeamLimit";

        public string Help => "Sets the team limit for every team.";

        public string Syntax => "-setTeamLimit {limit}";

        public string Permission => "admin";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            if (msg.Channel.Id != Convert.ToUInt64(File.ReadAllText(Program.channelLocation)))
            {
                string tcID = File.ReadAllText(Program.channelLocation);

                eb.AddField("Locked!", $"**This bot can only be used in <#{tcID}>**");
                eb.Color = Color.Red;

                var embedLocked = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLocked);
                return;
            }

            if (parameters.Length != 1)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }

            int limit;

            try
            {
                Console.WriteLine("triggered");
                limit = Convert.ToInt32(parameters[0]);
            }
            catch(Exception e)
            {
                Console.WriteLine("triggered error");
                await msg.Channel.SendMessageAsync($"**Something went wrong. Make sure your input doesnt contain anything else than numbers:** `{parameters[0]}`");
                Console.WriteLine(e);
                return;
            }


            Console.WriteLine("triggered 2");
            File.WriteAllText(Program.teamLimitLocation, "");
            File.WriteAllText(Program.teamLimitLocation, $"{limit}");

            eb.AddField("Success!", $"**The Team Limit was set to {limit}**");
            eb.Color = Color.Green;

            efb.IconUrl = msg.Author.GetAvatarUrl();
            efb.Text = DateTime.Now.ToString();

            eb.WithFooter(efb);

            var embed = eb.Build();

            await msg.Channel.SendMessageAsync("", embed: embed);
        }
    }

    class teamLeader : IDiscordCommand
    {
        public string Name => "TeamLeader";

        public string Help => "Assign a user to team leader";

        public string Syntax => "-teamLeader {@username}";

        public string Permission => "admin";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            string role = Config.leaderRole;

            var uRole = Convert.ToUInt64(role);

            string targetId = msg.MentionedUsers.Count == 1 ? msg.MentionedUsers.First().Id.ToString() : parameters[0];
            SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;
            SocketGuildUser target = server.Users.FirstOrDefault(x => x.Id.ToString() == targetId);

            var lRole = server.GetRole(uRole);

            if(target.Roles.Contains(lRole))
            {
                eb.AddField("Error!", $"**{target.Mention} already has this role.**");
                eb.Color = Color.Red;

                efb.IconUrl = target.GetAvatarUrl();
                efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embedI = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedI);

                return;
            }

            if (msg.Channel.Id != Convert.ToUInt64(File.ReadAllText(Program.channelLocation)))
            {
                string tcID = File.ReadAllText(Program.channelLocation);

                eb.AddField("Locked!", $"**This bot can only be used in <#{tcID}>**");
                eb.Color = Color.Red;

                var embedLocked = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLocked);
                return;
            }

            if (parameters.Length != 1)
            {
                eb.AddField("**Wrong command usage**", $"**{Syntax}**");
                eb.Color = Color.Red;

                var embedWU = eb.Build();

                var wrongUsage = await msg.Channel.SendMessageAsync("", embed: embedWU);
                await Task.Delay(5000);
                await wrongUsage.DeleteAsync();
                return;
            }

            if (target.IsBot)
            {
                await msg.Channel.SendMessageAsync("**Disclaimer:** This user is a bot, but to experimental reasons we won't do anything about that. :D");
            }

            await target.AddRoleAsync(lRole);

            eb.AddField("Success!", $"**{target.Mention} was added to the {lRole.Mention} role!**");
            eb.Color = Color.Green;

            efb.IconUrl = target.GetAvatarUrl();
            efb.Text = DateTime.Now.ToString();

            eb.WithFooter(efb);

            var embed = eb.Build();

            await msg.Channel.SendMessageAsync("", embed: embed);
        }
    }

    class Invite : IDiscordCommand
    {
        public string Name => "invite";

        public string Help => "Invites a user to the specified role.";

        public string Syntax => "-invite {@user} {@role}";

        public string Permission => "default";

        public async Task ExecuteAsync(SocketUserMessage msg, string[] parameters)
        {
            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder efb = new EmbedFooterBuilder();

            string leaderRole = Config.leaderRole;

            var uRole = Convert.ToUInt64(leaderRole);

            string targetId = msg.MentionedUsers.Count == 1 ? msg.MentionedUsers.First().Id.ToString() : parameters[0];
            SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;
            SocketGuildUser target = server.Users.FirstOrDefault(x => x.Id.ToString() == targetId);

            var lRole = server.GetRole(uRole);

            SocketGuildUser gUser = (SocketGuildUser)msg.Author;

            if (msg.Channel.Id != Convert.ToUInt64(File.ReadAllText(Program.channelLocation)))
            {
                string tcID = File.ReadAllText(Program.channelLocation);

                eb.AddField("Locked!", $"**This bot can only be used in <#{tcID}>**");
                eb.Color = Color.Red;

                var embedLocked = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLocked);
                return;
            }

            if (!gUser.Roles.Contains(lRole))
            {
                eb.AddField("No permission!", $"You need the {lRole.Mention} role to execute this command!");
                eb.Color = Color.Red;

                efb.IconUrl = gUser.GetAvatarUrl();
                efb.Text = efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embedR = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedR);

                return;
            }

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

            //Parse Role ID
            string r1 = parameters[1].Replace("<", "");
            string r2 = r1.Replace("&", "");
            string r3 = r2.Replace("@", "");
            string RoleID = r3.Replace(">", "");
            //replace this with a foreach statement

            var roleOut = Convert.ToUInt64(RoleID);

            var RoleL = server.GetRole(roleOut);

            SocketGuildUser author = (SocketGuildUser)msg.Author;

            if(RoleL.Id == lRole.Id)
            {
                await msg.Channel.SendMessageAsync("**You can't add anyone to the Leader role!**");
                return;
            }

            if(!author.Roles.Contains(RoleL))
            {
                eb.AddField("Error!", $"{author.Mention} you can't invite an user to a role you don't have! If this is a mistake please contact a moderator!");

                eb.Color = Color.Red;

                efb.IconUrl = author.GetAvatarUrl();
                efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embedA = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedA);

                return;
            }

            if(target.Roles.Contains(RoleL))
            {
                eb.AddField("Error!", $"{target.Mention} already has the {RoleL.Mention} role!");

                eb.Color = Color.Red;

                efb.IconUrl = author.GetAvatarUrl();
                efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embedT = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedT);

                return;
            }

            int teamLimit = Convert.ToInt32(File.ReadAllText(Program.teamLimitLocation));

            if(RoleL.Members.Count() >= teamLimit)
            {
                eb.Title = "Limit reached.";

                eb.AddField("Limit reached!", $"**This role has already reached it's limit of {teamLimit} members!**");
                eb.Color = Color.Red;

                efb.IconUrl = msg.Author.GetAvatarUrl();
                efb.Text = DateTime.Now.ToString();

                eb.WithFooter(efb);

                var embedLimit = eb.Build();

                await msg.Channel.SendMessageAsync("", embed: embedLimit);

                return;
            }

            await target.AddRoleAsync(RoleL);

            eb.Title = "Invite";

            eb.AddField("User", $"{target.Mention}");
            eb.AddField("Role", $"{RoleL.Mention}");
            eb.AddField("Invited by", $"{msg.Author.Mention}");
            eb.AddField("Team Members", $"{RoleL.Members.Count()}");

            eb.Color = RoleL.Color;

            eb.ImageUrl = target.GetAvatarUrl();

            efb.IconUrl = msg.Author.GetAvatarUrl();
            efb.Text = $"Invited by {msg.Author.Mention}";

            var embed = eb.Build();

            await msg.Channel.SendMessageAsync($"", embed: embed);
        }
    }
}
