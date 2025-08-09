using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace Utilities
{

    public class ScpSpawnEntry
    {
        public RoleTypeId Role;
        public float Chance;
    }
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;

        public string JoinedMessage { get; set; } = "{player} joined the server";
        public string LeftMessage { get; set; } = "{player} left the server";
        public string RoundStartedMessage { get; set; } = "";
        public ushort MaxScp { get; set; } = 2;

        public ushort RoleInfoMessageDuration { get; set; } = 6;
        public Dictionary<string, string> RoleMessages { get; set; } = new Dictionary<string, string>()
        {
            {"Scientist", "Escape the facility, cooperate with the guards"},
            {"ClassD", "Escape the facility, cooperate with the Chaos Insurgency (greens)"},
            {"SCP", "Eliminate everything that moves"},
            {"MTF", "Eliminate SCPs, escort scientists, enemies: Chaos Insurgency"},
            {"Chaos", "Rescue Class D, enemies: MTF, guards, scientists"}
        };

        public bool PowerOutage { get; set; } = true;
        public float PowerOutageTimeout { get; set; } = 3f; // Minutes
        public float PowerOutageRollInterval { get; set; } = 1f;  // Minutes
        public float PowerOutageDurationMin { get; set; } = 1f; // Seconds
        public float PowerOutageDurationMax { get; set; } = 10f; // Seconds
        public float PowerOutageChance { get; set; } = 0.25f;

        public bool Scenario { get; set; } = true;
        public float ScenarioDoorOpenTimeout { get; set; } = 5f; // Seconds

        public float NonDefaultNpcChance { get; set; } = 0.2f;
    }
}
