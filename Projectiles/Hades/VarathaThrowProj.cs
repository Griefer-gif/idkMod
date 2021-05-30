using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Idkmod.Projectiles.Hades
{
    public class VarathaThrowProj : ModProjectile
    {
        public override void SetDefaults()
        {
			projectile.width = 18;               //The width of projectile hitbox
			projectile.height = 18;              //The height of projectile hitbox
			projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 20;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 600; //og: 10         //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			aiType = ProjectileID.BoneJavelin;           
			projectile.penetrate = 1;
		}

        public override void AI()
        {
			//projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			
		}
    }
}
