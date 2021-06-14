using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Idkmod.Projectiles.Hades;

namespace Idkmod.Items.Weapons.Bows.Coronacht
{
    public class CoronachtProj : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Bows/Coronacht/CoronachtItem";

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
        private bool attackR = false;
        public override bool PreAI()
        {
            int arrow = ModContent.ProjectileType<CoronachtArrow>();
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            UpdatePlayerVisuals(player, rrp);
            projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
            if (player.channel)
            {
                projectile.Center = player.Center;
                counter++;
                if (counter == 40)
                {
                    attackR = true;
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
                if (projectile.owner == Main.myPlayer)
                {
                    if (attackR && counter <= 60)
                    {
                        //timed arrow
                        //idk how to fix the speed, but this is ok
                        Projectile.NewProjectile(projectile.Center, projectile.velocity * 200, arrow, projectile.damage * 2, projectile.knockBack, projectile.owner, 1);
                    }
                    else
                    {
                        //everything else
                        Projectile.NewProjectile(projectile.Center, projectile.velocity * 100, arrow, projectile.damage, projectile.knockBack, projectile.owner);

                    }
                }
                projectile.active = false;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
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
        }
    }
}
    

