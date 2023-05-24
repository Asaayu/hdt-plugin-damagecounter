using System;
using System.Collections.Generic;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;

namespace hdt_plugin_damagecounter
{
    public class PluginFunctions
    {
        internal static int GetTotalBoardDamage(IEnumerable<Entity> entities)
        {
            int totalDamage = 0;
            int weaponDamage = 0;
            Entity hero = null;

            foreach (Entity entity in entities)
            {
                if (entity.IsHero)
                {
                    hero = entity;
                    continue;
                }

                // Ignore dormant minions
                if (entity.HasTag(GameTag.DORMANT)) continue;

                int damage = entity.Attack;
                if (entity.HasTag(GameTag.MEGA_WINDFURY)) damage *= 4;
                else if (entity.HasTag(GameTag.WINDFURY)) damage *= 2;

                if (entity.IsWeapon) weaponDamage += Math.Max(damage, 0);

                totalDamage += Math.Max(damage, 0);
            }

            if (hero != null)
                totalDamage += Math.Max(hero.Attack - weaponDamage, 0);

            return Math.Max(totalDamage, 0);
        }

        internal static Entity GetHeroEntity(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.IsHero) return entity;
            }

            return null;
        }
    }
}
