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
using idkmod.Projectiles.Minions;
using idkmod.Buffs.Minions;

namespace Idkmod.Items.Weapons.Summons
{
	class PsyPoop : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Psy Poop");
			Tooltip.SetDefault("Summons a Psy Fly that blocks and reflects projectiles");
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
			item.scale = 2f;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Red;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<PsyFlyBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<PsyFlyMinion>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;

			if (!player.GetModPlayer<BlPlayer>().PsyFlyBuff)
			{
				return true;
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			
			var quote = new TooltipLine(mod, "", "'The God Fly!'")
			{
				overrideColor = Color.Purple
			};
			var tt = new TooltipLine(mod, "", "Only one can be spawned at a time");
			tooltips.Add(tt);
			tooltips.Add(quote);
		}

	}
}
