using System;
using System.Windows.Controls;
using System.Windows.Media;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace hdt_plugin_damagecounter
{
    public class PluginMain
    {
        private static readonly SolidColorBrush WhiteBrush = Brushes.White;
        private static readonly SolidColorBrush RedBrush = Brushes.Red;

        private static void SetLabelPosition(Label label, double posX, double posY, double width, double height, double fontSize)
        {
            Canvas.SetLeft(label, CoreAPI.OverlayCanvas.ActualWidth * posX);
            Canvas.SetTop(label, CoreAPI.OverlayCanvas.ActualHeight * posY);
            label.Width = CoreAPI.OverlayCanvas.ActualWidth * width;
            label.Height = CoreAPI.OverlayCanvas.ActualHeight * height;
            label.FontSize = Math.Max(10, CoreAPI.OverlayCanvas.ActualHeight * fontSize);
        }

        private static void ResetCanvas()
        {
            DamageCounter.playerDamageLabel.Content = "0 ⚔️";
            DamageCounter.opponentDamageLabel.Content = "0 ⚔️";

            DamageCounter.playerDamageLabel.Foreground = WhiteBrush;
            DamageCounter.opponentDamageLabel.Foreground = WhiteBrush;
        }

        internal static void UpdateDamageCalculator()
        {
            var playerBoard = CoreAPI.Game.Player.Board;
            var opponentBoard = CoreAPI.Game.Opponent.Board;

            var playerHero = PluginFunctions.GetHeroEntity(playerBoard);
            var opponentHero = PluginFunctions.GetHeroEntity(opponentBoard);

            if (playerHero == null || opponentHero == null) return;

            var possiblePlayerDamage = PluginFunctions.GetTotalBoardDamage(playerBoard);
            var possibleOpponentDamage = PluginFunctions.GetTotalBoardDamage(opponentBoard);

            var playerHealth = playerHero.Health + playerHero.GetTag(GameTag.ARMOR);
            var opponentHealth = opponentHero.Health + opponentHero.GetTag(GameTag.ARMOR);

            // If the labels aren't on the canvas, call the OnGameStart method to set them up
            if (!CoreAPI.OverlayCanvas.Children.Contains(DamageCounter.playerDamageLabel)) OnGameStart();

            DamageCounter.playerDamageLabel.Content = $"{possiblePlayerDamage} ⚔️";
            DamageCounter.opponentDamageLabel.Content = $"{possibleOpponentDamage} ⚔️";

            DamageCounter.playerDamageLabel.Foreground = possiblePlayerDamage >= opponentHealth ? RedBrush : WhiteBrush;
            DamageCounter.opponentDamageLabel.Foreground = possibleOpponentDamage >= playerHealth ? RedBrush : WhiteBrush;
        }

        internal static void OnPlayerPlay(Card _) => UpdateDamageCalculator();

        internal static void OnTurnStart(ActivePlayer _) => UpdateDamageCalculator();

        internal static void OnGameStart()
        {
            ResetCanvas();
            if (CoreAPI.Game.IsBattlegroundsMatch) {
                OnGameEnd();
                return;
            }
            CoreAPI.OverlayCanvas.Children.Add(DamageCounter.playerDamageLabel);
            CoreAPI.OverlayCanvas.Children.Add(DamageCounter.opponentDamageLabel);
            UpdateDamageCalculator();
        }

        internal static void OnGameEnd()
        {
            CoreAPI.OverlayCanvas.Children.Remove(DamageCounter.playerDamageLabel);
            CoreAPI.OverlayCanvas.Children.Remove(DamageCounter.opponentDamageLabel);
        }

        internal static void OnGameUpdate()
        {
            if (CoreAPI.Game.IsBattlegroundsMatch) return;

            SetLabelPosition(
                DamageCounter.playerDamageLabel,
                DamageCounter.playerPosX,
                DamageCounter.playerPosY,
                DamageCounter.infoWidth,
                DamageCounter.infoHeight,
                DamageCounter.infoFontSize
            );

            SetLabelPosition(
                DamageCounter.opponentDamageLabel,
                DamageCounter.opponentPosX,
                DamageCounter.opponentPosY,
                DamageCounter.infoWidth,
                DamageCounter.infoHeight,
                DamageCounter.infoFontSize
            );

            if (CoreAPI.Game.CurrentMode == Mode.GAMEPLAY)
            {
                UpdateDamageCalculator();
            }   
            else
            {
                OnGameEnd();
            }
        }
    }
}
