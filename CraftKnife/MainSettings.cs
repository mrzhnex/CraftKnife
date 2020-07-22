﻿using EXILED;

namespace CraftKnife
{
    public class MainSettings : Plugin
    {
        public override string getName => nameof(CraftKnife);
        public SetEvents SetEvents { get; set; }

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Events.PlayerDeathEvent += SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Events.PlayerDeathEvent -= SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}