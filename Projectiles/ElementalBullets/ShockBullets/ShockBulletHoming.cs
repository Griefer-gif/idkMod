using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using idkmod.Projectiles.ElementalBullets.ShockBullets;
using Terraria.ID;

namespace idkmod.Projectiles.ElementalBullets.ShockBullets
{
    class ShockBulletHoming : ShockBullet
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shock bullet homing");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);

			aiType = ProjectileID.ChlorophyteBullet;
		}
	}
}
