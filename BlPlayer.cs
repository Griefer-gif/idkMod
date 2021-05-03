﻿using System;
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
        public bool Corrosive;
        public bool Fire;
        public bool Shock;
        public bool Slagg;
        public int shieldMaxHealth;
        public int shieldCHealth;
        public bool gotHit;
        public int HitTimer;
        public int shieldsEquipped;
        public const int maxUses = 1;
        public int LifeCrystal;
        Random random = new Random();

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
            Corrosive = false;
            Fire = false;
            Shock = false;
            Slagg = false;

            player.statLifeMax2 += LifeCrystal * 25;
        }

        public override void UpdateDead()
        {
            Corrosive = false;
            Fire = false;
            Shock = false;
            Slagg = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (Corrosive)
            {
                int DamageDB = 20;

                if (player.statLife < player.statLifeMax / 2)
                {
                    DamageDB = 35;
                    if (Slagg)
                    {
                        DamageDB = 45;
                    }

                    if (player.statLife < player.statLifeMax / 4)
                    {
                        DamageDB = 50;
                        if (Slagg)
                        {
                            DamageDB = 75;
                        }

                        if (player.statLife < player.statLifeMax / 8)
                        {
                            DamageDB = 100;
                            if (Slagg)
                            {
                                DamageDB = 150;
                            }

                            if (player.statLife < player.statLifeMax / 15)
                            {
                                DamageDB = 200;
                                if (Slagg)
                                {
                                    DamageDB = 300;
                                }
                            }
                        }
                    }
                }

                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }

                if (DamageDB < 2)
                {
                    DamageDB = 2;
                }

                player.lifeRegen -= DamageDB;
                
            }

            if (Shock)
            {
                int r = random.Next(10);
                if (r == 0)
                {
                    player.velocity -= player.oldVelocity;
                }
                else
                {
                    if (Slagg)
                    {
                        player.velocity -= player.oldVelocity / 2;
                    }
                    else
                    {
                        player.velocity -= player.oldVelocity / 4;
                    }
                }
            }

            if (Fire)
            {
                int DamageDB = 40;
                if (Slagg)
                {
                    DamageDB = 50;
                }

                if (player.statLife < player.statLifeMax / 2)
                {
                    DamageDB = 35;
                    if (Slagg)
                    {
                        DamageDB = 40;
                    }

                    if (player.statLife < player.statLifeMax / 4)
                    {
                        DamageDB = 20;
                        if (Slagg)
                        {
                            DamageDB = 35;
                        }

                        if (player.statLife < player.statLifeMax / 8)
                        {
                            DamageDB = 16;
                            if (Slagg)
                            {
                                DamageDB = 20;
                            }

                            if (player.statLife < player.statLifeMax / 15)
                            {
                                DamageDB = 10;
                                if (Slagg)
                                {
                                    DamageDB = 16;
                                }
                            }
                        }
                    }
                }

                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegen -= DamageDB;
            }
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
