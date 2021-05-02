using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Projectiles
{
	public class FireBulletHoming : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfury V2");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);
			aiType = ProjectileID.ChlorophyteBullet;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.ChlorophyteBullet;
			return true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 600, false);
		}

    }
}