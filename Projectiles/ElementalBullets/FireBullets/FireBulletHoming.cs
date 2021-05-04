using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.ElementalBullets.FireBullets
{
	public class FireBulletHoming : FireBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Incendiary bullet homing");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);
			
			aiType = ProjectileID.ChlorophyteBullet;
		}

    }
}