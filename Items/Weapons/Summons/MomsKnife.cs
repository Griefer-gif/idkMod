using Idkmod.Buffs.Minions;
using Idkmod.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Summons
{
	public class MomsKnife : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mom's Knife");
			Tooltip.SetDefault("Summons a knife to stab you enemies\nWhen the knife gets close to an enemy it has to charge up before going into a murderous spread");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<MomsKnifeBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<MomsKnifeMinion>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;
			if (!player.GetModPlayer<idkPlayer>().MomsKnifeBuff)
			{
				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofFright, 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
