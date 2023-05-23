using System;
using System.Windows.Controls;
using System.Windows.Media;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace hdt_plugin_damagecounter
{
    public class PluginMain
    {
        private static void SetCanvasPosition()
        {
            if (!CoreAPI.OverlayCanvas.Children.Contains(DamageCounter.playerDamageLabel) || !CoreAPI.OverlayCanvas.Children.Contains(DamageCounter.opponentDamageLabel))
                return;

            // Set the player damage label position
            Canvas.SetLeft(DamageCounter.playerDamageLabel, CoreAPI.OverlayCanvas.ActualWidth * DamageCounter.playerPosX);
            Canvas.SetTop(DamageCounter.playerDamageLabel, CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.playerPosY);
            DamageCounter.playerDamageLabel.Width = CoreAPI.OverlayCanvas.ActualWidth * DamageCounter.infoWidth;
            DamageCounter.playerDamageLabel.Height = CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.infoHeight;
            DamageCounter.playerDamageLabel.FontSize = Math.Max(10, CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.infoFontSize);

            // Set the opponent damage label position
            Canvas.SetLeft(DamageCounter.opponentDamageLabel, CoreAPI.OverlayCanvas.ActualWidth * DamageCounter.opponentPosX);
            Canvas.SetTop(DamageCounter.opponentDamageLabel, CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.opponentPosY);
            DamageCounter.opponentDamageLabel.Width = CoreAPI.OverlayCanvas.ActualWidth * DamageCounter.infoWidth;
            DamageCounter.opponentDamageLabel.Height = CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.infoHeight;
            DamageCounter.opponentDamageLabel.FontSize = Math.Max(10, CoreAPI.OverlayCanvas.ActualHeight * DamageCounter.infoFontSize);
        }

        private static void ResetCanvas()
        {
            // Text
            DamageCounter.playerDamageLabel.Content = "0 Dmg";
            DamageCounter.opponentDamageLabel.Content = "0 Dmg";

            // Color
            DamageCounter.playerDamageLabel.Foreground = Brushes.White;
            DamageCounter.opponentDamageLabel.Foreground = Brushes.White;
        }

        internal static void UpdateDamageCalculator()
        {
            // Get all entities in the game
            var entities = CoreAPI.Game.Entities;

            // Get all minions on the player's board
            var playerBoard = CoreAPI.Game.Player.Board;

            // Get all minions on the opponent's board
            var opponentBoard = CoreAPI.Game.Opponent.Board;

            // Get the player's hero
            var playerHero = PluginFunctions.GetHeroEntity(entities, true);

            // Get the opponent's hero
            var opponentHero = PluginFunctions.GetHeroEntity(entities, false);

            // Check that both heros exist
            if (playerHero == null || opponentHero == null) return;

            // Get the total damage potential of the player's board
            var possiblePlayerDamage = PluginFunctions.GetTotalBoardDamage(playerBoard);

            // Get the total damage potential of the opponent's board
            var possibleOpponentDamage = PluginFunctions.GetTotalBoardDamage(opponentBoard);

            // Get the health of the player's hero
            var playerHealth = playerHero.Health + playerHero.GetTag(GameTag.ARMOR);

            // Get the health of the opponent's hero
            var opponentHealth = opponentHero.Health + opponentHero.GetTag(GameTag.ARMOR);

            DamageCounter.playerDamageLabel.Content = "" + possiblePlayerDamage + " Dmg";
            DamageCounter.opponentDamageLabel.Content = "" + possibleOpponentDamage + " Dmg";

            DamageCounter.playerDamageLabel.Foreground = possiblePlayerDamage >= opponentHealth ? Brushes.Red : Brushes.White;
            DamageCounter.opponentDamageLabel.Foreground = possibleOpponentDamage >= playerHealth ? Brushes.Red : Brushes.White;
        }

        internal static void OnPlayerPlay(Card _) { UpdateDamageCalculator(); }

        internal static void OnTurnStart(ActivePlayer _) { UpdateDamageCalculator(); }

        internal static void OnGameStart()
        {
            // Reset the UI
            ResetCanvas();

            // Add the damage labels to the overlay canvas
            CoreAPI.OverlayCanvas.Children.Add(DamageCounter.playerDamageLabel);
            CoreAPI.OverlayCanvas.Children.Add(DamageCounter.opponentDamageLabel);

            // Run the damage calculator
            UpdateDamageCalculator();
        }

        internal static void OnGameEnd()
        {
            // Remove the damage labels from the overlay canvas
            CoreAPI.OverlayCanvas.Children.Remove(DamageCounter.playerDamageLabel);
            CoreAPI.OverlayCanvas.Children.Remove(DamageCounter.opponentDamageLabel);
        }

        internal static void OnGameUpdate()
        {
            SetCanvasPosition();

            if (CoreAPI.Game.CurrentMode == Mode.GAMEPLAY)
                UpdateDamageCalculator();
        }
    }
}
