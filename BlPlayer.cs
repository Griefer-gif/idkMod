using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Idkmod
{
    public class BlPlayer : Terraria.ModLoader.ModPlayer
    {
        public int shieldMaxHealth;
        public int shieldCHealth;
        public bool gotHit;
        public int HitTimer;
        public int shieldsEquipped;
        public const int maxUses = 1;
        public int LifeCrystal;

        public override bool CloneNewInstances => true;

        public override void PreUpdate()
        {
            if (shieldsEquipped <= 0)
            {
                HitTimer = 0;
                shieldCHealth = 0;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //-----------------------------------
            //shield recharge code
            gotHit = true;

            HitTimer = 0;

            if (shieldCHealth < 1)
            {
                shieldCHealth = 0;
            }

            if (shieldCHealth >= 1)
            {
                shieldCHealth -= damage;

                if (shieldCHealth < 1)
                {
                    shieldCHealth = 0;
                }

                return false;
            }
            else
            {
                gotHit = true;
            }

            //----------------------------
            return true;
        }

        public override void ResetEffects()
        {
            shieldMaxHealth = 0;
            shieldsEquipped = 0;
            
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
                //{"LifeCrystal",LifeCrystal }
                [nameof(LifeCrystal)] = LifeCrystal,

            };
        }

        public override void Load(TagCompound tag)
        {
            LifeCrystal = tag.GetInt("LifeCrystal");
        }

    }
}
