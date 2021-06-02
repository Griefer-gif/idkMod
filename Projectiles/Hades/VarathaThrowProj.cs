using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Idkmod.Items.Weapons.Spears.Varatha;
using Microsoft.Xna.Framework.Graphics;

namespace Idkmod.Projectiles.Hades
{
    public class VarathaThrowProj : ModProjectile
    {
        //ai 0 = traveling
        //ai 1 = hit something
        //ai 2 = going back to player
        public override void SetDefaults()
        {
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 20;
			projectile.scale = 1.3f;
            projectile.timeLeft = 300;
            projectile.light = 1f;
		}

        public override void AI()
        {
			Player projOwner = Main.player[projectile.owner];
            Vector2 direction = new Vector2();
            float inertia = 10f;
            float speed = 30f;
            if (projectile.timeLeft <= 30 || projOwner.HeldItem.type != ModContent.ItemType<VarathaItem>())
            {
                projectile.ai[0] = 2;
            }

            if (projectile.ai[0] == 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                projectile.velocity *= 1.03f;
            }
            else if(projectile.ai[0] == 1)
            {
                projectile.velocity = Vector2.Zero;
            }
            else if(projectile.ai[0] == 2)
            {
                direction = projOwner.Center - projectile.Center;
                direction.Normalize();
                direction *= speed;
                projectile.tileCollide = false;
                projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                projectile.timeLeft = 9999;

            }

            if (projectile.Distance(projOwner.Center) < 30 && projectile.ai[0] != 0)
            {
                projectile.Kill();
            }

            projOwner.GetModPlayer<idkPlayer>().varthaStoredProj = projectile.whoAmI;
			
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(projectile.ai[0] == 0)
            {
                projectile.ai[0] = 1;
                projectile.timeLeft = 600;
            }
            
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 50; i++)
            {
                Dust.NewDust(projectile.position, 30, 30, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Black, Scale: 1f);
                Dust.NewDust(projectile.position, 35, 35, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Aqua, Scale: 0.8f);
            }
            Main.player[projectile.owner].GetModPlayer<idkPlayer>().varthaStoredProj = -1;
            base.Kill(timeLeft);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.ai[0] == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(projectile.position, 16, 16, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Aqua, Scale: 0.5f);
                    Dust.NewDust(projectile.position, 30, 30, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Black, Scale: 0.7f);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(projectile.position, 16, 16, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Aqua, Scale: 0.3f);
                }
            }
            
            return true;
        }
    }
}
