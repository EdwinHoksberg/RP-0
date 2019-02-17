﻿using System;
using System.Collections.Generic;
using System.Text;
using KSP;
using UnityEngine;

namespace RP0
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] { GameScenes.EDITOR, GameScenes.SPACECENTER })]
    public class ToolingManager : ScenarioModule
    {
        #region Fields

        protected static ToolingDatabase database = new ToolingDatabase();

        public bool toolingEnabled = true;

        #region Instance

        private static ToolingManager _instance = null;
        public static ToolingManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #endregion

        #region Overrides and Monobehaviour methods

        public override void OnAwake()
        {
            base.OnAwake();

            if (_instance != null)
            {
                GameObject.Destroy(_instance);
            }
            _instance = this;

            GameEvents.OnGameSettingsApplied.Add(LoadSettings);
            GameEvents.onGameStateLoad.Add(LoadSettings);
        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            ToolingDatabase.Load(node.GetNode("Tooling"));

        }
        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            ToolingDatabase.Save(node.AddNode("Tooling"));
        }
        public void OnDestroy()
        {
            GameEvents.OnGameSettingsApplied.Remove(LoadSettings);
            GameEvents.onGameStateLoad.Remove(LoadSettings);
        }

        #endregion

        #region Methods

        protected void LoadSettings(ConfigNode n)
        {
            LoadSettings();
        }

        protected void LoadSettings()
        {
            toolingEnabled = HighLogic.CurrentGame.Parameters.CustomParams<RP0Settings>().IsToolingEnabled;
        }

        #endregion
    }
}
