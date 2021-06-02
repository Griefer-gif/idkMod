using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Idkmod.Projectiles.Hades;

namespace Idkmod.Items.Weapons.Spears.Varatha
{
    public class VarathaSwingProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 999999;
            projectile.scale = 1.1f;
        }

        int counter = 0;
        int stacks = 0;
        public override bool PreAI()
        {
           
            
            Player player = Main.player[projectile.owner];
            projectile.velocity = new Vector2(0, -1);

            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            UpdatePlayerVisuals(player, rrp);
            if (player.channel)
            {
                //player.ChangeDir(projectile.direction);
                //player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                counter++;
                if (counter % 40 == 0 && stacks < 3)
                {
                    stacks++;
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
                if (stacks > 0)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<VarathaSpinAttack>(), projectile.damage * stacks, projectile.knockBack, projectile.owner, stacks);
                }
                else
                {
                    Projectile.NewProjectile(player.position, Vector2.Normalize(Main.MouseWorld - player.position) * 10f, ModContent.ProjectileType<VarathaThrowProj>(), projectile.damage * 3, projectile.knockBack, projectile.owner);
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
            projectile.Center = playerHandPos - new Vector2(0, 60);

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
        }
    }
}
