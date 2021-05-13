using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.Minions
{
	//ported from my tAPI mod because I'm lazy
	public class PsyFlyReflectProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			projectile.Name = "PsyFlyReflectProj";
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);

			aiType = ProjectileID.ChlorophyteBullet;
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
			var dustIndex = Dust.NewDust(projectile.oldPosition, 10, 10, DustID.PurpleTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, Scale: 0.8f);
			Main.dust[dustIndex].noGravity = true;
			return true;
		}
		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.ChlorophyteBullet;
			return true;
		}
	}
}