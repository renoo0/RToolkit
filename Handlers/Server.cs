using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using UnityEngine;
using MEC;
using System.Collections.Generic;

namespace Utilities.Handlers
{
    public class ServerEvents
    {
        private static Config Config => Plugin.Instance.Config;
        private CoroutineHandle lightCheckCoroutine;

        private string blackoutCassieMessage = "<size=0> PITCH_.2 .G4 .G4 PITCH_.9 ATTENTION PITCH_.6  .G2 PITCH_.8 JAM_027_4 . PITCH_.15 .G4 .G4 PITCH_9999</size><color=#d64542>Attention, <color=#f5e042>TEMPORARY POWER OUTAGE...<split><size=0> PITCH_.9 GENERATORS PITCH_.7";
        public void OnWaitingForPlayers()
        {
            Log.Info("Waiting for players...");
        }

        public void OnRoundStarted()
        {
            string message = Config.RoundStartedMessage;
            Map.Broadcast(6, message);

            lightCheckCoroutine = Timing.RunCoroutine(LightShutdownRoutine());

            if (Config.Scenario)
            {
                Room scp173Room = Room.Get(RoomType.Lcz173);
                Log.Info(scp173Room.Position);

                Vector3 classDOffset = new Vector3(15f, 12f, 8f);
                Vector3 scientistOffset = new Vector3(8f, 12f, 8f);

                foreach (Player player in Player.List)
                {
                    float randX = UnityEngine.Random.Range(0.5f, 1.5f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
                    float randZ = UnityEngine.Random.Range(0.5f, 1.5f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
                    Vector3 randomOffset = new Vector3(randX, 0f, randZ);

                    Vector3 spawnOffset;

                    if (player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                    {
                        spawnOffset = classDOffset;
                    }
                    else if (player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
                    {
                        spawnOffset = scientistOffset;
                    }
                    else
                        continue;

                    Vector3 rotatedMainOffset = scp173Room.Rotation * spawnOffset;
                    Vector3 rotatedRandomOffset = scp173Room.Rotation * randomOffset;

                    Vector3 finalSpawnPosition = scp173Room.Position + rotatedMainOffset + rotatedRandomOffset;
                    player.Teleport(finalSpawnPosition);

                    Timing.WaitForSeconds(Config.ScenarioDoorOpenTimeout);

                    foreach (var door in scp173Room.Doors)
                    {
                        if (!door.IsOpen)
                        {
                            door.IsOpen = true;
                            door.Unlock();
                        }
                    }
                }
            }

        }

        private IEnumerator<float> LightShutdownRoutine()
        {
            yield return Timing.WaitForSeconds(Config.PowerOutageTimeout * 60);

            while (Round.IsStarted)
            {
                float chance = UnityEngine.Random.value;

                if (chance < Config.PowerOutageChance)
                {
                    float outageDuration = UnityEngine.Random.Range(Config.PowerOutageDurationMin, Config.PowerOutageDurationMax);

                    Map.TurnOffAllLights(outageDuration);
                    Cassie.Message(blackoutCassieMessage);
                    Log.Info($"Lights have been shut down for {outageDuration}");

                    yield return Timing.WaitForSeconds(outageDuration);

                    Log.Info("Lights have been turned back on");
                    
                }

                yield return Timing.WaitForSeconds(Config.PowerOutageRollInterval * 60);
            }
        }
    }
}
