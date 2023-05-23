using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;

namespace hdt_plugin_damagecounter
{
    public class DamageCounter : IPlugin
    {
        // Font size - % of overlay height
        internal static double infoFontSize = 0.019;

        // Info box size - % of overlay size
        internal static double infoHeight = 0.04;
        internal static double infoWidth = 0.08;

        // Player info box position - % of overlay size
        internal static double playerPosX = 0.77;
        internal static double playerPosY = 0.50;

        // Opponent info box position - % of overlay size
        internal static double opponentPosX = 0.77;
        internal static double opponentPosY = 0.38;

        // Damage labels
        internal static Label playerDamageLabel = new Label
        {
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Effect = new DropShadowEffect
            {
                Color = Color.FromRgb(0, 0, 0),
                Direction = 315,
                ShadowDepth = 0,
                Opacity = 1,
                BlurRadius = 5
            }
        };

        internal static Label opponentDamageLabel = new Label
        {
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Effect = new DropShadowEffect
            {
                Color = Color.FromRgb(0, 0, 0),
                Direction = 315,
                ShadowDepth = 0,
                Opacity = 1,
                BlurRadius = 5
            }
        };

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            GameEvents.OnGameStart.Add(PluginMain.OnGameStart);
            GameEvents.OnGameEnd.Add(PluginMain.OnGameEnd);
            GameEvents.OnTurnStart.Add(PluginMain.OnTurnStart);
            GameEvents.OnPlayerPlay.Add(PluginMain.OnPlayerPlay);
        }

        public void OnUnload()
        {
            PluginMain.OnGameEnd();
        }

        public void OnButtonPress() {}

        public void OnUpdate()
        {
            PluginMain.OnGameUpdate();
        }

        public string Name => "Damage Counter";

        public string Description => "Calculates and displays the total damage potential of each side of the game board possible.\n\nUse this information to quickly determine if you or your opponent have lethal.";

        public string ButtonText => "No Settings";

        public string Author => "Asaayu";

        public Version Version => new Version(0, 0, 1);

        public MenuItem MenuItem => null;
    }
}