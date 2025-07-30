using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using CommandSystem;
using RemoteAdmin;

namespace Utilities.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class HelloWorld : ICommand
    {
        public string Command { get; } = "rhello";
        public string[] Aliases { get; } = {"helloworld"};
        public string Description { get; } = "A command that sayy hello to the world";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Map.Broadcast(3, "Sperma");
            if (sender is PlayerCommandSender player)
            {
                response = $"Hello {player.Nickname}";
                return false;
            }
            else
            {
                response = "World!";
                return true;
            }
        }
    }
}
