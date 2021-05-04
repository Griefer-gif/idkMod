using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.ElementalBullets.CorrosiveBullets
{
	public class CorrosiveBulletHoming : CorrosiveBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive bullet homing");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);

			aiType = ProjectileID.ChlorophyteBullet;
		}

	}
}