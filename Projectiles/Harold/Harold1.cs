
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
			projectile.width = 8;               
			projectile.height = 8;              
			projectile.aiStyle = 1;             
			projectile.friendly = true;         
			projectile.hostile = false;         
			projectile.ranged = true;           
			projectile.penetrate = 20;          
			projectile.timeLeft = 10; //og: 10  
			projectile.alpha = 255;             
			projectile.light = 0.5f;            
			projectile.ignoreWater = true;      
			projectile.tileCollide = true;      
			projectile.extraUpdates = 1;        
			aiType = ProjectileID.Bullet;       
			projectile.penetrate = 1;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//dust, wow fancy
			int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 10, default(Color), 1f);
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