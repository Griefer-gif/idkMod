using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles.ElementalBullets.SlaggBullets;

namespace Idkmod.Items.Weapons.Guns.BL2
{
	class Slagga : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slagga");
			Tooltip.SetDefault("fires 3 ichor shots per shot, ammo type is ignored.");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 0.6f;
			item.useTime = 2; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 2; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 15;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 50f; // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int typeElement;

			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(2);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				if(type == ProjectileID.ChlorophyteBullet){
					typeElement = ModContent.ProjectileType<SlaggBulletHoming>();
				}
				else
				{
					typeElement = ModContent.ProjectileType<SlaggBullet>();
				}

				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, typeElement, damage, knockBack, player.whoAmI);
			}

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var Element = new TooltipLine(mod, "", "Slag weapon");
			Element.overrideColor = Color.Purple;
			var ElementE = new TooltipLine(mod, "", "Slagged enemies take more damage from other elements");
			ElementE.overrideColor = Color.Purple;
			tooltips.Add(Element);
			tooltips.Add(ElementE);
            var quote = new TooltipLine(mod, "", "'blagaga'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}

        public override void HoldItem(Player player)
        {
			
        }
    }
}