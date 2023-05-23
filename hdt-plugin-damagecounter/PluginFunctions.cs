using System;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using HearthDb.Enums;
using System.Collections.Generic;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace hdt_plugin_damagecounter
{
    public class PluginFunctions
    {
        internal static int GetTotalBoardDamage(IEnumerable<Entity> entities, bool ignoreSleeping = false)
        {
            int totalDamage = 0;
            int weaponDamage = 0;

            // Create a list to store the heros in
            Entity hero = null;

            // Loop through each entity in the list
            foreach (Entity entity in entities)
            {
                if (entity.IsHero)
                {
                    hero = entity;
                    continue;
                };
                
                // Get the attack value of the entity
                var damage = entity.Attack;

                // Check if the entity has a windfury effect
                if (entity.HasTag(GameTag.MEGA_WINDFURY)) damage *= 4;
                else if (entity.HasTag(GameTag.WINDFURY)) damage *= 2;

                if (entity.IsWeapon) weaponDamage += Math.Max(damage, 0);

                // Add the attack value of the entity to the total damage
                totalDamage += Math.Max(damage, 0);
            };

            // Remove the weapon damage from the hero attack value to prevent double counting
            if (hero != null) totalDamage += Math.Max(hero.Attack - weaponDamage, 0);

            // Return the total damage
            return Math.Max(totalDamage, 0);
        }

        internal static Entity GetHeroEntity(Dictionary<int, Entity> entities, bool player)
        {
            // Loop through each entity in the list of entities
            foreach (Entity entity in entities.Values)
            {
                // Skip the entity if it is not a hero
                if (!entity.IsHero) continue;

                // Check if the entity is the player's hero or the opponent's hero
                if (player && entity.IsControlledBy(CoreAPI.Game.Player.Id)) return entity;
                if (!player && entity.IsControlledBy(CoreAPI.Game.Opponent.Id)) return entity;
                
                // Skip the entity if it is not the player's hero or the opponent's hero
                continue;
            };

            // Return null if no hero was found
            return null;
        }
    }
}
