using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles
{
	public class HighBulletNonCol : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Non collision High speed bullet");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BulletHighVelocity);
			projectile.penetrate = 1;
			aiType = ProjectileID.BulletHighVelocity;
			projectile.tileCollide = false;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.BulletHighVelocity;
			return true;
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
			Dust.NewDust(projectile.oldPosition, projectile.width, projectile.height, DustID.YellowTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Color.White, 0.8f);
			return true;
		}

	}
}