using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Bows.TitanSouls
{
    public class TitanSlayerHoldout  : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Bows/TitanSouls/TitanSlayer";

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 999999;
        }

        private int counter = 0;
       
        public override bool PreAI()
        {
            int arrow = ModContent.ProjectileType<TitanSlayerArrow>();
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            
            UpdatePlayerVisuals(player, rrp);

            if (player.channel)
            {
                int counterMaxValue = 60;
                projectile.Center = player.Center;
                if(counter < counterMaxValue + 1)//+1 so that the next if doesnt get stuck
                {
                    counter++;
                }

                if (counter == counterMaxValue)
                {
                   
                    //Main.NewText("stacks");
                    Main.PlaySound(SoundID.MaxMana, (int)projectile.position.X, (int)projectile.position.Y);
                    for (int i = 0; i < 100; i++)
                    {
                        DoDustEffect(player.Center, 40f);
                    }
                }
            }
            else
            {
                if (projectile.owner == Main.myPlayer && counter >= 20)
                {
                    Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center);

                    Main.PlaySound(SoundID.Item5, player.position);
                    Projectile.NewProjectile(player.Center, (velocity * 30f), arrow, projectile.damage * 2, projectile.knockBack, projectile.owner, counter);
                }
                projectile.active = false;
            }

            return true;
        }
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.BoneTorch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .5f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            projectile.Center = playerHandPos;

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
        }
    }

    public class TitanSlayerArrow : ModProjectile
    {
        private int timer = 0;
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TitanSlayerArrow");     
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        
        }

        public override void SetDefaults()
        {
            projectile.width = 8;           
            projectile.height = 8;                   
            projectile.friendly = true;     
            projectile.hostile = false;     
            projectile.ranged = true;
            projectile.timeLeft = 9999;         
            projectile.light = 0.5f;        
            projectile.ignoreWater = true;  
            projectile.tileCollide = true;  
            projectile.penetrate = 9999;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Vector2 direction;
            float inertia = 20f;
            float speed = 20f;
            Player player = Main.player[projectile.owner];
            if (projectile.ai[1] == 0)
            {
                float maxTimecounter = projectile.ai[0] * 2;
                timer++;
                if (timer < maxTimecounter)
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                }
                else
                {
                    if (projectile.Distance(player.Center) <= 20f)
                    {
                        Main.PlaySound(SoundID.Grab, player.position);
                        projectile.Kill();
                    }
                    projectile.velocity = Vector2.Zero;
                    if (projectile.rotation != MathHelper.Pi)
                    {
                        if (projectile.rotation >= MathHelper.Pi - 0.2 && projectile.rotation <= MathHelper.Pi + 0.2)
                        {
                            projectile.rotation = MathHelper.Pi;
                        }
                        else
                        {
                            if (projectile.rotation >= MathHelper.Pi)
                            {
                                projectile.rotation -= 0.3f;
                            }
                            else
                            {
                                projectile.rotation += 0.3f;
                            }
                        }
                    }
                    else
                    {
                        const double amp = 20;
                        const double freq = 0.07;
                        projectile.position.Y += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq);
                    }
                }

                if (projectile.ai[0] > 0)
                {
                    projectile.ai[0] -= 2;
                }
            }
            else if(projectile.ai[1] == 1)
            {
                direction = player.Center - projectile.Center;
                direction.Normalize();
                direction *= speed;
                projectile.tileCollide = false;
                projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                projectile.timeLeft = 9999;
                if(projectile.Distance(player.Center) <= 16f)
                {
                    projectile.Kill();
                }
                
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Item10, projectile.position);
            return false;
        }
    }

    public class ArrowMagnet : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Bows/TitanSouls/TitanSlayer";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 9999;
            projectile.light = 0.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 9999;
            projectile.extraUpdates = 0;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            for (int i = 0; Main.projectile.Length > i; i++)
            {
                if (Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TitanSlayerArrow>() && Main.projectile[i].active)
                {
                    projectile.velocity = Vector2.Normalize(Main.projectile[i].Center - player.Center);
                    if (player.channel)
                    {
                        if (Main.projectile[i].velocity == Vector2.Zero)
                        {
                            Main.projectile[i].ai[1] = 1;
                        }

                        projectile.rotation = projectile.velocity.ToRotation();
                        player.ChangeDir(projectile.direction);
                    }
                    else
                    {
                        Main.projectile[i].ai[1] = 0;
                    }
                    
                }
                
            }
            if (!player.channel)
            {
                projectile.active = false;
                player.heldProj = -1;
            }
            projectile.Center = player.Center;
            
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
    }
 }

