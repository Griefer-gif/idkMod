
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using On.Terraria.GameContent.Achievements;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.Harold
{
	public class Harold1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CCBullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			projectile.width = 8;               //The width of projectile hitbox
			projectile.height = 8;              //The height of projectile hitbox
			projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 20;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 10; //og: 10         //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
			aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
			projectile.penetrate = 1;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			//Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			//for (int k = 0; k < projectile.oldPos.Length; k++)
			//{
			//	Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
			//	Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			//	spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			//}

			int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 55, 0f, 0f, 10, default(Color), 1f);
			Main.dust[dustIndex].noGravity = true;

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 25; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 10, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1f;
			}
			// Fire Dust spawn
			for (int i = 0; i < 40; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 10, default(Color), 1.6f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 1f;
				dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 10, default(Color), 1.6f);
				Main.dust[dustIndex].velocity *= 1f;
				//Main.dust[dustIndex].noGravity = true;
			}
			
			// reset size to normal width and height.
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 10;
			projectile.height = 10;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);
		}

		public override void Kill(int timeLeft)
		{
			if(timeLeft == 0)
            {
				Vector2 speed = projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(3));
				Vector2 speed2 = projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(-3));

				Projectile.NewProjectile(projectile.oldPosition, speed, ModContent.ProjectileType<Harold2>(), projectile.damage, 0, 0);
				Projectile.NewProjectile(projectile.oldPosition, speed2, ModContent.ProjectileType<Harold2>(), projectile.damage, 0, 0);
				Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);
			}
		}
	}

	
}