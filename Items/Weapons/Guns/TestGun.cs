using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles.ElementalBullets.CorrosiveBullets;

namespace Idkmod.Items.Weapons.Guns
{
	class TestGun : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("g u n");
			Tooltip.SetDefault("Yep");
		}

		public override void SetDefaults()
		{
			item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 0.8f;
			item.useTime = 10; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 10; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 15;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item41; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 5f; // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			type = ModContent.ProjectileType<idkmod.Projectiles.SwordProj.SwordStickProj0>();

			// Add random Rotation

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, typeElement, damage, knockBack, player.whoAmI);
			Main.PlaySound(SoundID.Item41, position);

			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "test gun lul"));
			var quote = new TooltipLine(mod, "", "'Poke simp gun'")
			{
				overrideColor = Color.Red
			};
			tooltips.Add(quote);
		}
	}
}
