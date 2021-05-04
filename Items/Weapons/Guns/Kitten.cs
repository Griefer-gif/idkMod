using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles.ElementalBullets.FireBullets;

namespace Idkmod.Items.Weapons.Guns
{
	class Kitten : ModItem
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kitten");
			Tooltip.SetDefault("Fires fire bullets in a spread");
		}

		public override void SetDefaults()
		{
			//(60 ticks == 1 second.)
			item.damage = 10;
			item.crit = 0;
			item.ranged = true;
			item.width = 20;
			item.height = 20;
			item.scale = 0.8f;
			item.useTime = 20;
			item.useAnimation = 0;
			item.reuseDelay = 10;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 50f;
			item.useAmmo = AmmoID.Bullet;

		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			base.PickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int typeElement;

			timer++;
			if (timer == 5)
			{
				player.statLife += 1; 
				timer = 0;
			}

			if (type == ProjectileID.ChlorophyteBullet)
			{
				typeElement = ModContent.ProjectileType<FireBulletHoming>();
			}
			else
			{
				typeElement = ModContent.ProjectileType<FireBullet>();
			}

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 35f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				
				position += muzzleOffset;
			}

			int numberProjectiles = 3; //og = 15
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // 2 degree spread.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, typeElement, damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

			}

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Heals 1 HP every 5 shots"));

            var quote = new TooltipLine(mod, "", "'We're all mad here. I'm mad. You're mad.'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}


	}
}