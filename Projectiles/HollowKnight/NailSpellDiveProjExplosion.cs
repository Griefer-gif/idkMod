using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Projectiles.HollowKnight
{
	class NailSpellDiveProjExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dive Explosion");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{

			projectile.width = 250;               //The width of projectile hitbox
			projectile.height = 80;              //The height of projectile hitbox
			//projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.melee = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 30;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			//projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;          //Can the projectile collide with tiles?
			projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
			//aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
		}

        public override void AI()
        {
			Main.player[projectile.owner].Center = projectile.Center + new Vector2(0, 20);
			Main.player[projectile.owner].immune = true;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			for (int i = 0; i < 100; i++)
			{
				var dust = Dust.NewDust(projectile.oldPosition, projectile.width, projectile.height, DustID.WhiteTorch, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Scale: 1.5f);
				Main.dust[dust].noGravity = true;

			}

			return true;
		}
	}
}
