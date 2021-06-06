using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Projectiles.HollowKnight
{
    public class OldNailBlade : ModProjectile
    {
        //2 swings still not implemented
        //frame 1 - 7 = first swing
        // 7 - 14 = second swing
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 14;
        }

        public override void SetDefaults()
        {
            projectile.width = 70;
            projectile.height = 70;
            projectile.friendly = true;
            projectile.penetrate = 999;
            projectile.tileCollide = false;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.timeLeft = 7; //7 so it matches the animation, change the animation loop too if you change this
        }
        public override void AI()
        {
            //position part
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 20f;
            projectile.Center = rrp;

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            //animation loop
            if (++projectile.frameCounter >= 1)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 14)
                {
                    projectile.frame = 0;
                }
            }

            //this prevents the sprite from messing up when facing left
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.Pi;
            }
        }
    }
}
