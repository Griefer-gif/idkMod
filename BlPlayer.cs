using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Idkmod
{
    public class BlPlayer : Terraria.ModLoader.ModPlayer
    {
        public float shieldMaxHealth = 0;
        public float shieldCHealth = 0;
        public float shieldRRate = 0;
        public bool gotHit;
        public float shieldTimer = 0;

        public const int maxUses = 1;
        public int LifeCrystal;


        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            shieldCHealth -= (float)damage;
            if(shieldCHealth <= 0)
            {
                shieldCHealth = 0;
            }
            gotHit = true;
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            npc.AddBuff(39, 1000);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (shieldCHealth > 0)
            {
                damage = -1;
            }
        }

        public override void ResetEffects()
        {
            shieldMaxHealth = 0;
            shieldCHealth = 0;
            shieldRRate = 0;
            gotHit = false;
            shieldTimer = 0;
            player.statLifeMax2 += LifeCrystal * 25;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(LifeCrystal);
            packet.Send(toWho, fromWho);
        }
        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"LifeCrystal",LifeCrystal }
            };
        }

        public override void Load(TagCompound tag)
        {
            LifeCrystal = tag.GetInt("LifeCrystal");
        }

    }
}
