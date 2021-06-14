using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Idkmod.Projectiles.Hades
{
    public class CoronachtArrow : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CoronachtArrow");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;//The length of old position to be recorded
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
			aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
			projectile.penetrate = 1;

		}

        public override void AI()
        {
			if(projectile.ai[0] == 1)
            {
				projectile.velocity *= 1.02f;
			}
            else
            {
				projectile.velocity *= 1.01f;
			}
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

			if(projectile.ai[0] == 1)
            {
				projectile.penetrate = 30;
				for(int i = 0; i < 5; i++)
                {
					DoDustEffect(projectile.Center, 40f, 10, 15);
				}
            }
            else
			{
				int dust = Dust.NewDust(projectile.oldPosition, projectile.width, projectile.height, DustID.PurpleTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Color.White, 1f);
				Main.dust[dust].noGravity = true;
			}
			

			return true;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position);
			return true;                                                                                    
		}

        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.PurpleTorch, Scale: 1.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
	}
}
