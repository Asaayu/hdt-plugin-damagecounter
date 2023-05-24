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
        public Version Version => new Version(0, 2, 2);

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

        // Player Damage Label
        internal static Label playerDamageLabel;

        // Opponent Damage Label
        internal static Label opponentDamageLabel;

        // Damage Label Text
        internal static string damageLabelText = "{damage} ⚔️";

        public void OnLoad()
        {
            playerDamageLabel = CreateDamageLabel();
            opponentDamageLabel = CreateDamageLabel();

            GameEvents.OnGameStart.Add(PluginMain.OnGameStart);
            GameEvents.OnGameEnd.Add(PluginMain.OnGameEnd);
            GameEvents.OnTurnStart.Add(PluginMain.OnTurnStart);
            GameEvents.OnPlayerPlay.Add(PluginMain.OnPlayerPlay);
        }

        public void OnUnload()
        {
            PluginMain.OnGameEnd();
        }

        public void OnButtonPress() { }

        public void OnUpdate()
        {
            PluginMain.OnGameUpdate();
        }

        public string Name => "Damage Counter";

        public string Description => "Calculates and displays the total damage potential of each side of the game board possible.\n\nUse this information to quickly determine if you or your opponent have lethal.";

        public string ButtonText => "No Settings";

        public string Author => "Asaayu";

        public MenuItem MenuItem => null;

        private Label CreateDamageLabel()
        {
            return new Label
            {
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 315,
                    ShadowDepth = 0,
                    Opacity = 1,
                    BlurRadius = 8,
                    RenderingBias = RenderingBias.Quality,
                },
                Padding = new Thickness(5)  // Add padding to create space between the text and shadow
            };
        }
    }
}
