using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles.ElementalBullets.ExplosiveBullets;

namespace Idkmod.Items.Weapons.Guns.BL2
{
	class UnkemptHarold : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unkempt Harold");
			Tooltip.SetDefault("fires 3 shots that split into two more shots");
		}

		public override void SetDefaults()
		{
			item.damage = 50; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 10; // hitbox width of the item
			item.height = 10; // hitbox height of the item
			item.useTime = 1; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 1; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 30;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 0.5f; //og:16f // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float offsetY = 0;
			float offsetX = 0;
			if (speedY > 2 || speedY < -2)
			{
				offsetX = 15;
			}
            else
            {
				offsetY = 15;
            }

			type = ModContent.ProjectileType<Rocket>();
			//3 rockets generated a bit far from each other
			Projectile.NewProjectile(position.X + offsetX, position.Y + offsetY, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 1);
			Projectile.NewProjectile(position.X - offsetX, position.Y- offsetY, speedX, speedY, type, damage, knockBack, player.whoAmI);

			//type = ModContent.ProjectileType<idkmod.Projectiles.Harold.Harold1>();
			//float numberProjectiles = 3;
			//float rotation = MathHelper.ToRadians(2);
			//position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			//for (int i = 0; i < numberProjectiles; i++)
			//{
			//	Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .1f; // Watch out for dividing by 0 if there is only 1 projectile.
			//	Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			//}
			//return false;
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 3);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var Element = new TooltipLine(mod, "", "EXPLOSIVE WEAPON");
			Element.overrideColor = Color.Yellow;
			var ElementE = new TooltipLine(mod, "", "EXPLOSIVE WEAPONS... EXPLODE!");
			ElementE.overrideColor = Color.Yellow;
			tooltips.Add(Element);
			tooltips.Add(ElementE);
			var quote = new TooltipLine(mod, "", "'Did I fire six shots, or only five? Three? Seven. Whatever.'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}
	}
}
