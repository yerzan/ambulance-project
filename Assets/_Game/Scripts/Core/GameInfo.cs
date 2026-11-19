namespace LastCall.Core
{
    /// <summary>
    /// Static product facts shared across systems. Gameplay balance does NOT belong here
    /// (use ScriptableObjects in Assets/_Game/Data).
    /// </summary>
    public static class GameInfo
    {
        public const string ProductName = "LAST CALL";

        /// <summary>Minimum medic players for a session (solo is for testing only; GDD §18.1).</summary>
        public const int MinMedics = 1;

        /// <summary>Minimum medic players for a regular Career Co-op match (GDD §3.1).</summary>
        public const int MinCoopMedics = 2;

        /// <summary>Maximum medic players in Career Co-op (GDD §3.1).</summary>
        public const int MaxMedics = 4;

        /// <summary>First release: patients are NPC-only (Patient Online is a future version).</summary>
        public const bool PlayerControlledPatientEnabled = false;
    }
}
