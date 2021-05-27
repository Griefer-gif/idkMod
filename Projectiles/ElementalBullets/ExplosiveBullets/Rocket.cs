using Idkmod.Items.Weapons.Guns.BL2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.ElementalBullets.ExplosiveBullets
{
	public class Rocket : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ExplosiveBullet");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			projectile.damage = 0;

			projectile.width = 15;               //The width of projectile hitbox
			projectile.height = 15;              //The height of projectile hitbox
			//projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?  
			projectile.timeLeft = 300;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			//projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			//projectile.extraUpdates = 1;//Set to above 0 if you want the projectile to update multiple time in a frame
			projectile.timeLeft = 60;
			projectile.penetrate = 1;
			//aiType = ProjectileID.Bullet;           //Act exactly like default Bullet

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			var dustIndex = Dust.NewDust(projectile.oldPosition, 10, 10, DustID.GreenTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, Scale: 0.8f);
			Main.dust[dustIndex].noGravity = true;
			return true;
		}

        public override void AI()
        {
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowTorch, Scale: 2f);
			Main.dust[dust].noGravity = true;
			const double amp = 10;
			const double freq = 0.15;
			//needs 2 checks so it doesnt screw up at oposite axis
			Vector2 NVelocity = Vector2.Normalize(projectile.velocity);
			//checks if its vertically
			if (projectile.ai[0] == 1 && (NVelocity.Y > 0.5 || NVelocity.Y < -0.5))
            {
				projectile.position.X += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq);
			}
            else if(NVelocity.Y > 0.5 || NVelocity.Y < -0.5)
            {
				projectile.position.X += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq) * -1;
			}

			//checks if the proj is travelling horizontally
			if (projectile.ai[0] == 1 && !(NVelocity.Y > 0.5 || NVelocity.Y < -0.5))
			{
				projectile.position.Y += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq);
			}
			else if (!(NVelocity.Y > 0.5 || NVelocity.Y < -0.5))
			{
				projectile.position.Y += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq) * -1;
			}

			projectile.rotation = projectile.velocity.ToRotation();
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			
		}

		public override void Kill(int timeLeft)
		{ 
			if(Main.player[projectile.owner].HeldItem.type == ModContent.ItemType<UnkemptHarold>())
            {
				if (projectile.timeLeft <= 1)
				{
					Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<MiniRocket>(), projectile.damage, projectile.knockBack, projectile.owner, 1);
					Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<MiniRocket>(), projectile.damage, projectile.knockBack, projectile.owner, 0);
				}
			}
			
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position);
		}
	}
}
