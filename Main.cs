using System;
using Exiled.API.Enums;
using Exiled.API.Features;

using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;

namespace Utilities
{
    public class Plugin : Plugin<Config> 
    {
        public static Plugin Instance { get; private set; }

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        private Handlers.PlayerEvents player;
        private Handlers.ServerEvents server;

        public Plugin()
        {

        }

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            Instance = null;
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            player = new Handlers.PlayerEvents();
            server = new Handlers.ServerEvents();

            Server.WaitingForPlayers += server.OnWaitingForPlayers;
            Server.RoundStarted += server.OnRoundStarted;

            Player.Left += player.OnLeft;
            Player.Verified += player.OnVerified;
            Player.ChangingRole += player.OnChangingRole;
            Player.Spawned += player.OnSpawned;
        }

        public void UnregisterEvents()
        {
            Server.WaitingForPlayers -= server.OnWaitingForPlayers;
            Server.RoundStarted -= server.OnRoundStarted;

            Player.Left -= player.OnLeft;
            Player.Verified -= player.OnVerified;
            Player.ChangingRole -= player.OnChangingRole;
            Player.Spawned -= player.OnSpawned;

            player = null;
            server = null;
        }
    }
}
