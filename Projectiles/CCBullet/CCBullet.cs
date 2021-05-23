using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles
{
	public class CCBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CCBullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;//The length of old position to be recorded
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
			projectile.timeLeft = 600; //og: 10         //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
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

    }
}