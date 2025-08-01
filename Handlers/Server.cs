using Exiled.API.Features;
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

        private string blackoutCassieMessage = "<size=0> PITCH_.2 .G4 .G4 PITCH_.9 ATTENTION ALL PITCH_.6 PERSONNEL .G2 PITCH_.8 JAM_027_4 . PITCH_.15 .G4 .G4 PITCH_9999</size><color=#d64542>Attention, <color=#f5e042>all personnel...<split><size=0> PITCH_.9 GENERATORS PITCH_.7 IN THE PITCH_.85 FACILITY HAVE BEEN PITCH_.8 DAMAGED PITCH_.2 .G4 .G4 PITCH_9999</size><color=#d67d42>Generators in <color=#f5e042>the facility <color=#d67d42>have been <color=#d64542>damaged.<split><size=0> PITCH_.8 THE FACILITY PITCH_.9 IS GOING THROUGH PITCH_.85 A BLACK OUT PITCH_.15 .G4 .G4 PITCH_9999</size><color=#d64542><color=#f5e042>The facility <color=#d67d42>is going through a <color=#000000>blackout.";
        public void OnWaitingForPlayers()
        {
            Log.Info("Waiting for players...");
        }

        public void OnRoundStarted()
        {
            string message = Config.RoundStartedMessage;
            Map.Broadcast(6, message);

            lightCheckCoroutine = Timing.RunCoroutine(LightShutdownRoutine());
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
