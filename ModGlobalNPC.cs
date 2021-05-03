using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod
{
    class ModGlobalNPC : GlobalNPC
    {
        Random random = new Random();
        public override bool InstancePerEntity => true;

        public bool Corrosive;
        public bool Fire;
        public bool Shock;
        public bool Slagg;

        public override void ResetEffects(NPC npc)
        {
            Corrosive = false;
            Fire = false;
            Shock = false;
            Slagg = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            
            if (Corrosive)
            {
                int DamageDB = 20;

                if(npc.life < npc.lifeMax / 2)
                {
                    DamageDB = 35;
                    if (Slagg)
                    {
                        DamageDB = 45;
                    }

                    if (npc.life < npc.lifeMax / 4)
                    {
                        DamageDB = 50;
                        if (Slagg)
                        {
                            DamageDB = 75;
                        }

                        if (npc.life < npc.lifeMax / 8)
                        {
                            DamageDB = 100;
                            if(Slagg)
                            {
                                DamageDB = 150;
                            }

                            if (npc.life < npc.lifeMax / 15)
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

                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= DamageDB;

                if (damage < 2)
                {
                    damage = 2;
                }
            }

            if(Shock)
            {
                int r = random.Next(10);
                if(r == 0)
                {
                    npc.velocity -= npc.oldVelocity;
                }
                else
                {
                    if (Slagg)
                    {
                        npc.velocity -= npc.oldVelocity / 2;
                    }
                    else
                    {
                        npc.velocity -= npc.oldVelocity / 4;
                    }
                }
            }

            if(Fire)
            {
                int DamageDB = 26;
                if (Slagg)
                {
                    DamageDB = 32;
                }

                if (npc.life < npc.lifeMax / 2)
                {
                    DamageDB = 20;
                    if (Slagg)
                    {
                        DamageDB = 26;
                    }

                    if (npc.life < npc.lifeMax / 4)
                    {
                        DamageDB = 16;
                        if (Slagg)
                        {
                            DamageDB = 20;
                        }

                        if (npc.life < npc.lifeMax / 8)
                        {
                            DamageDB = 10;
                            if (Slagg)
                            {
                                DamageDB = 16;
                            }

                            if (npc.life < npc.lifeMax / 15)
                            {
                                DamageDB = 6;
                                if (Slagg)
                                {
                                    DamageDB = 10;
                                }
                            }
                        }
                    }
                }

                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= DamageDB;

                if (damage < 2)
                {
                    damage = 2;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Corrosive)
            {
                if (Main.rand.Next(4) < 3)
                {
                    //Dust generation
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4,DustID.CursedTorch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                //Color of the light that the npc emits
                Lighting.AddLight(npc.position, 0f, 2.55f, 0f);
            }

            if(Slagg)
            {
                //Dust generation
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Venom, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                //Color of the light that the npc emits
                Lighting.AddLight(npc.position, 1.02f, 0f, 1.04f);
            }

            if (Shock)
            {
                //Dust generation
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.BlueFairy, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                //Color of the light that the npc emits
                Lighting.AddLight(npc.position, 0.51f, 153f, 255f);
            }

            if(Fire)
            {
                //Dust generation
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Fire, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                //Color of the light that the npc emits
                Lighting.AddLight(npc.position, 2.55f, 0.50f, 0f);
            }
        }
    }
}
