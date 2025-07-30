using System;
using Exiled.API.Enums;
using Exiled.API.Features;

using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;

namespace Utilities
{
    public class Plugin : Plugin<Config> 
    {
        public static readonly Plugin Instance = new Plugin();

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        private Handlers.PlayerEvents player;
        private Handlers.ServerEvents server;

        public Plugin()
        {

        }

        public override void OnEnabled()
        {
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            player = new Handlers.PlayerEvents();
            server = new Handlers.ServerEvents();

            Server.WaitingForPlayers += server.OnWaitingForPlayers;
            Server.RoundStarted += server.OnRoundStarted;

            Player.Left += player.OnLeft;
            Player.Joined += player.OnJoined;
            Player.ChangingRole += player.OnChangingRole;
        }

        public void UnregisterEvents()
        {
            Server.WaitingForPlayers -= server.OnWaitingForPlayers;
            Server.RoundStarted -= server.OnRoundStarted;

            Player.Left -= player.OnLeft;
            Player.Joined -= player.OnJoined;
            Player.ChangingRole -= player.OnChangingRole;

            player = null;
            server = null;
        }
    }
}
