using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using idkmod.Projectiles;
using idkmod.Buffs;
using Microsoft.Xna.Framework.Graphics;
using idkmod.Dusts;
using Idkmod.Buffs.HK;
using Idkmod.Projectiles.HollowKnight;

namespace Idkmod
{
    public class idkPlayer : ModPlayer
    {
        public Queue<Projectile> psyFlyQueue  = new Queue<Projectile>();
        public int varthaStoredProj;
        public bool NailSpell1;
        public bool NailSpell1UP;
        public bool NailSpell2;
        public bool NailSpell2UP;
        public bool NailSpellCD;
        public bool PsyFlyBuff;
        public bool MomsKnifeBuff;
        public bool DarkArtsBuff;
        public bool DarkArts;
        public bool DarkArtsCD;
        public bool DAActive;
        public int DATimer = 0;
        private int DACPosition = 0;
        public bool Corrosive;
        public bool Fire;
        public bool Shock;
        public bool Slagg;
        public int shieldMaxHealth;
        public int shieldCHealth;
        public bool gotHit = true;
        public int HitTimer;
        public int shieldsEquipped;
        public const int maxUses = 1;
        public int LifeCrystal;
        public List<NPC> DANpcs = new List<NPC>();
        readonly Random random = new Random();


        public override bool CloneNewInstances => true;

        public override void PostUpdate()
        {
            if(PsyFlyBuff)
            {
                //Main.NewText(psyFlyQueue.Count);
                for (int i = 0; i < Main.projectile.Count(); i++)
                {
                    if (psyFlyQueue.Count > 0)
                    { 
                        if (psyFlyQueue.Peek().Distance(player.Center) > 200f || psyFlyQueue.Peek().active == false)
                        {

                            {
                                psyFlyQueue.Dequeue();
                            }
                        }
                    }

                    if (Main.projectile[i].Distance(player.Center) < 200f && psyFlyQueue.Contains(Main.projectile[i]) == false && Main.projectile[i].hostile)
                    {
                        if(psyFlyQueue.Count < 5)
                        {
                            psyFlyQueue.Enqueue(Main.projectile[i]);
                        }

                    }
                }
            }
            if (!player.HasBuff(ModContent.BuffType<DarkArtsBuff>()) && DANpcs.Count > 0)
            {
                DATimer++;
                DAReset(DANpcs, DATimer);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //Dark Arts
            if(DarkArtsBuff)
            {
                player.immune = false;
                return false;
            }

            //-----------------------------------
            //shield recharge code
            gotHit = true;

            HitTimer = 0;

            if (shieldCHealth < 0)
            {
                shieldCHealth = 0;
            }

            if (shieldCHealth >= 1 && !player.immune)
            {
                int Cdamage = damage - player.statDefense;
                if(Cdamage <= 0)
                {
                    Cdamage = 1;
                }

                if(!DarkArtsBuff)
                {
                    shieldCHealth -= Cdamage;
                }
                

                if (shieldCHealth < 0)
                {
                    shieldCHealth = 0;
                }

                damage = 1;
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
            if(shieldMaxHealth == 0)
            {
                HitTimer = 0;
                shieldCHealth = 0;
            }

            shieldMaxHealth = 0;
            shieldsEquipped = 0;
            Corrosive = false;
            Fire = false;
            Shock = false;
            Slagg = false;
            DarkArts = false;
            DarkArtsBuff = false;
            DarkArtsCD = false;
            PsyFlyBuff = false;
            MomsKnifeBuff = false;
            NailSpell1 = false;
            NailSpell1UP = false;
            NailSpell2 = false;
            NailSpell2UP = false;
            NailSpellCD = false;
            //varthaStoredProj = 0;

            player.statLifeMax2 += LifeCrystal * 25;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //effects for when the player does not already have the buff
            if (Idkmod.DarkArtsHotKey.JustPressed && player.GetModPlayer<idkPlayer>().DarkArts && !DarkArtsBuff && !DarkArtsCD)
            {
                for (int i = 0; i < 75; i++)
                {
                    int dust = Dust.NewDust(player.position, 16, 16, DustID.Smoke, Main.rand.Next(-30, 30), Main.rand.Next(-30, 30), 0, Color.Black, 1);
                    Main.dust[dust].scale = 2f;
                    //Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].noGravity = true;
                }

                player.AddBuff(ModContent.BuffType<DarkArtsBuff>(), 300);
                player.AddBuff(ModContent.BuffType<DarkArtsCD>(), 1200);
            }
            //effects for when he already has the buff
            if (Idkmod.DarkArtsHotKey.JustPressed && player.HasBuff(ModContent.BuffType<idkmod.Buffs.DarkArtsBuff>()) && DANpcs.Count > 0)
            {
                //method is at end of the class
                //DAReset(DANpcs);
                player.ClearBuff(ModContent.BuffType<DarkArtsBuff>());

                for (int i = 0; i < 75; i++)
                {
                    int dust = Dust.NewDust(player.position, 16, 16, DustID.Smoke, 0f, 0f, 0, Color.Black, 1);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].velocity *= 1.5f;
                }
            }
            //hk nail spell 1 effects hotkey
            if(Idkmod.NailSpell1HK.JustPressed && player.GetModPlayer<idkPlayer>().NailSpell1 && !NailSpellCD)
            {
                Projectile.NewProjectile(player.Center, Vector2.Normalize(Main.MouseWorld - player.Center) * player.HeldItem.shootSpeed, ModContent.ProjectileType<NailSpell1Proj>(), player.HeldItem.damage * 2, 10, Main.myPlayer);
                player.AddBuff(ModContent.BuffType<NailSpellCD>(), 300);
            }
            //upgraded spell hotkey
            if (Idkmod.NailSpell1HK.JustPressed && player.GetModPlayer<idkPlayer>().NailSpell1UP && !NailSpellCD)
            {
                Projectile.NewProjectile(player.Center, Vector2.Normalize(Main.MouseWorld - player.Center) * player.HeldItem.shootSpeed, ModContent.ProjectileType<NailSpell1ProjUP>(), player.HeldItem.damage * 3, 10, Main.myPlayer);
                player.AddBuff(ModContent.BuffType<NailSpellCD>(), 600);
            }
            //Descending thing level 1
            if (Idkmod.NailSpell2HK.JustPressed && !NailSpellCD && player.GetModPlayer<idkPlayer>().NailSpell2 && (player.velocity.Y < -1 || player.velocity.Y > 1))
            {
                Projectile.NewProjectile(player.Center, new Vector2(0, 10), ModContent.ProjectileType<NailSpellDiveProj>(), player.HeldItem.damage * 3, 10, Main.myPlayer);
                player.AddBuff(ModContent.BuffType<NailSpellCD>(), 600);
            }
            //descending dark
            if (Idkmod.NailSpell2HK.JustPressed && !NailSpellCD && player.GetModPlayer<idkPlayer>().NailSpell2UP && (player.velocity.Y < -1 || player.velocity.Y > 1))
            {
                Projectile.NewProjectile(player.Center, new Vector2(0, 10), ModContent.ProjectileType<NailSpellDiveProjUP>(), player.HeldItem.damage * 5, 10, Main.myPlayer);
                player.AddBuff(ModContent.BuffType<NailSpellCD>(), 600);
            }
        }

        public override void UpdateDead()
        {
            PsyFlyBuff = false;
            Corrosive = false;
            Fire = false;
            Shock = false;
            Slagg = false;
            DarkArtsCD = false;
            DarkArts = false;
            DarkArtsBuff = false;
            HitTimer = 0;
            shieldCHealth = 0;
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

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(DarkArtsBuff)
            {
                if (Main.rand.NextBool(1))
                {
                    for(int i = 0; i < 2; i++)
                    {
                        int dust = Dust.NewDust(drawInfo.position, 30, 30, ModContent.DustType<VoidDust>(), Main.rand.Next(-10, 10), 0f, 0, Color.Black);
                        Main.dust[dust].scale = 1.5f;
                        Main.dust[dust].noGravity = true;
                    }
                }
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void Load(TagCompound tag)
        {
            LifeCrystal = tag.GetInt("LifeCrystal");
        }

        private void DAReset(List<NPC> DAnpcs, int timer)
        {
           
            Main.NewText(timer);
            if (timer % 30 == 0 && DACPosition < DAnpcs.Count() && DAnpcs.Count() > 1)
            {
                Texture2D tex = Idkmod.Instance.GetTexture("Items/Accessories/BindingOfIsaac/DarkArts/DATrailStrip");
                SpriteBatch sprite = Main.spriteBatch;
                sprite.Begin();
                
                for(int i = 0; i < DACPosition; i++)
                {
                    Vector2 start = DAnpcs[i].Center;
                    Vector2 end = DAnpcs[i + 1].Center;
                    int length = (int)(start - end).Length();
                   
                    Main.NewText("draw!" + i);
                    sprite.Draw(tex, start - Main.screenPosition, null, Color.Black, (end - start).ToRotation(), new Vector2(0f, tex.Height), new Vector2(length, 1) * 1, SpriteEffects.None, 0f);
                }
                
                sprite.End();
                DACPosition++;
            }
            else if(DACPosition >= DAnpcs.Count() - 1)
            {
                for (int i = 0; i < DAnpcs.Count; i++)
                {

                    int damage = (int)player.HeldItem.damage * 2;
                    DAnpcs[i].DelBuff(DAnpcs[i].FindBuffIndex(ModContent.BuffType<Shadowed>()));
                    DAnpcs[i].StrikeNPC(damage, 3, 1, crit: true);

                    //just so i dont get confused again, this random thing is because the dust explosion can have two variants, look at the velocity
                    int rand = random.Next(2);
                    for (int u = 15; u > 0; u--)
                    {

                        if (rand == 1)
                        {
                            int dust = Dust.NewDust(DANpcs[i].position, 16, 16, DustID.Smoke, -5, 5, 0, Color.Black, 2);
                            Main.dust[dust].velocity *= 1.5f;
                            Main.dust[dust].noGravity = true;
                            int dust2 = Dust.NewDust(DANpcs[i].position, 16, 16, DustID.Smoke, 5, -5, 0, Color.Black, 2);
                            Main.dust[dust2].velocity *= 1.5f;
                            Main.dust[dust2].noGravity = true;
                        }
                        else
                        {
                            int dust = Dust.NewDust(DANpcs[i].position, 16, 16, DustID.Smoke, 5, 5, 0, Color.Black, 2);
                            Main.dust[dust].velocity *= 1.5f;
                            Main.dust[dust].noGravity = true;
                            int dust2 = Dust.NewDust(DANpcs[i].position, 16, 16, DustID.Smoke, -5, -5, 0, Color.Black, 2);
                            Main.dust[dust2].velocity *= 1.5f;
                            Main.dust[dust2].noGravity = true;
                        }

                    }

                }
                DAnpcs.Clear();
                DACPosition = 0;
                DATimer = 0;
            }
        }

    }
}
