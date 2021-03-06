using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Guns.BL2
{
	class Maggie : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maggie");
			Tooltip.SetDefault("Fires 6 projectiles per shot.");
		}

		public int num = 0;

		public override void SetDefaults()
		{
			item.damage = 15; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.useTime = 2; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 4; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 4;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = false; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 6;
			for (int i = 1; i < numberProjectiles + 1; i++)
			{
				damage = (damage * 4) / (i * 2);
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5)); // 30 degree spread.
																											   
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				
			}
			
			//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BaneS1").WithVolume(.7f).WithPitchVariance(.5f));

			return false; // return false because we don't want tmodloader to shoot projectile
		}

		public override bool ConsumeAmmo(Player player)
		{
			return !(player.itemAnimation < item.useAnimation - 2);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
            //tooltips.Add(new TooltipLine(mod, "", "Converts Musket balls into high speed bullets."));
            var quote = new TooltipLine(mod, "", "'Monty's wife don't take no guff.'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}

	}
}