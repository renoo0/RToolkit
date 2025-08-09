using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities.Handlers
{
    public class PlayerEvents
    {
        private static Config Config => Plugin.Instance.Config;

        public void OnLeft(LeftEventArgs ev)
        {
            string message = Config.LeftMessage.Replace("{player}", ev.Player.Nickname);
            Map.Broadcast(4, message);
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            string message = Config.JoinedMessage.Replace("{player}", ev.Player.Nickname);
            Map.Broadcast(2, message);
        }

        private string GetRoleGroupKey(RoleTypeId role)
        {
            switch (role)
            {
                case RoleTypeId.ClassD:
                    return "ClassD";
                case RoleTypeId.Scientist:
                    return "Scientist";
            }

            switch (role.GetSide())
            {
                case Side.Scp:
                    return "SCP";
                case Side.Mtf:
                    return "MTF";
                case Side.ChaosInsurgency:
                    return "Chaos";
                default:
                    return "Default";
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (PlayerRoles.PlayerRolesUtils.GetTeam(ev.NewRole) == PlayerRoles.Team.SCPs && ev.NewRole != RoleTypeId.Scp0492)
            {
                int scpCount = Player.List.Count(p => p.Role.Team == PlayerRoles.Team.SCPs);
                Log.Info($"SCPs: {scpCount}");
                if (scpCount >= Config.MaxScp && (PlayerRoles.PlayerRolesUtils.GetTeam(ev.Player.Role) != PlayerRoles.Team.SCPs))
                {
                    ev.NewRole = PlayerRoles.RoleTypeId.ClassD;
                }
            }

            string roleKey = GetRoleGroupKey(ev.NewRole);
            Log.Info($"{ev.Player.Nickname} is changing role to {roleKey}");

            if (Config.RoleMessages.TryGetValue(roleKey, out string message))
            {
                ev.Player.Broadcast(Config.RoleInfoMessageDuration, message);
            }
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (PlayerRoles.PlayerRolesUtils.GetTeam(ev.Player.Role) == PlayerRoles.Team.SCPs && ev.Player.Role != RoleTypeId.Scp0492 && ev.Player.Role != RoleTypeId.Scp106)
            {
                System.Random rand = new System.Random();
                if (rand.NextDouble() < Config.NonDefaultNpcChance)
                {
                    RoleTypeId newRole = (rand.Next(2) == 0) ? RoleTypeId.Scp3114 : RoleTypeId.Scp096;

                    ev.Player.Role.Set(newRole, RoleSpawnFlags.None);
                }
            }
        }

        //public void OnInteractingDoor(InteractingDoorEventArgs ev)
        //{
        //    if (ev.IsAllowed == false)
        //    {
        //        ev.Player.Broadcast(3, "Brat nie może otworzyć drzwi");
        //        ev.Player.Kill(DamageType.Unknown);
        //    }
        //}
    }
}
