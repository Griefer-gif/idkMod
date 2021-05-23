using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles
{
	public class CCBulletHoming : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conference call bullet homing");
		}

		public override void SetDefaults()
		{
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

			Dust.NewDust(projectile.oldPosition, projectile.width, projectile.height, DustID.YellowTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Color.White, 0.3f);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.owner == Main.myPlayer)
			{

				Vector2 speed = new Vector2(180f, 0);
				Vector2 speed2 = new Vector2(-180f, 0);

				Vector2 diference = new Vector2(-800f, 0f);
				Vector2 diference2 = new Vector2(800f, 0f);

				Vector2 position = projectile.oldPosition + diference;
				Vector2 position2 = projectile.oldPosition + diference2;
				Projectile.NewProjectile(position, speed, ModContent.ProjectileType<HighBulletNonCol>(), damage, 0, 0, 0, 0);
				Projectile.NewProjectile(position2, speed2, ModContent.ProjectileType<HighBulletNonCol>(), damage, 0, 0, 0, 0);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{

				Vector2 speed = new Vector2(180f, 0);
				Vector2 speed2 = new Vector2(-180f, 0);

				Vector2 diference = new Vector2(-800f, 0f);
				Vector2 diference2 = new Vector2(800f, 0f);

				Vector2 position = projectile.oldPosition + diference;
				Vector2 position2 = projectile.oldPosition + diference2;
				Projectile.NewProjectile(position, speed, ModContent.ProjectileType<HighBulletNonCol>(), projectile.damage, 0, 0, 0, 0);
				Projectile.NewProjectile(position2, speed2, ModContent.ProjectileType<HighBulletNonCol>(), projectile.damage, 0, 0, 0, 0);
			}

			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position);
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.ChlorophyteBullet;
			return true;
		}
	}
}