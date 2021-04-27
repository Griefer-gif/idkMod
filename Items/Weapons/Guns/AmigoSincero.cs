using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Guns
{
	class AmigoSincero : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amigo Sincero");
			Tooltip.SetDefault("Damage ignores enemy defence");
		}

		public override void SetDefaults()
		{
			item.damage = 200; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.crit = 10;
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 1.2f;
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
			return new Vector2(-8, 0);
		}

        public override void HoldItem(Player player)
        {
			player.armorPenetration = 9999;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var quote = new TooltipLine(mod, "", "'A true friend can penetrate any barrier.'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}


	}
}