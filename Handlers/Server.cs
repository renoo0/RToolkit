using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Handlers
{
    public class ServerEvents
    {
        private static Config Config => Plugin.Instance.Config;
        public void OnWaitingForPlayers()
        {
            Log.Info("Waiting for players...");
        }

        public void OnRoundStarted()
        {
            string message = Config.RoundStartedMessage;
            Map.Broadcast(6, message);
        }
    }
}
