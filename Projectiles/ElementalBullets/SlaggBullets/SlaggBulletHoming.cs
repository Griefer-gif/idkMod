using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.ElementalBullets.SlaggBullets
{
	public class SlaggBulletHoming : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag bullet homing");
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

			var dustIndex = Dust.NewDust(projectile.oldPosition, 10, 10, DustID.PurpleTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, Scale: 0.8f);
			Main.dust[dustIndex].noGravity = true;
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.ChlorophyteBullet;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Burning>(), 600, false);
		}

	}
}