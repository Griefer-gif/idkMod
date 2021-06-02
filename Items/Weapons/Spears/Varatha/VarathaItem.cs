using Idkmod.Projectiles.Hades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Spears.Varatha
{
    public class VarathaItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varatha");
			Tooltip.SetDefault("The Eternal Spear");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 30;
			//item.useTurn = true;
			item.channel = true;
			item.shootSpeed = 3.7f;
			item.knockBack = 6.5f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(silver: 10);
			item.melee = true;
			item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.

			item.UseSound = SoundID.Item1;
			
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<VarathaThrowProj>()] > 0)
			{
				if(player.altFunctionUse != 2)
                {
					Main.projectile[player.GetModPlayer<idkPlayer>().varthaStoredProj].ai[0] = 2;
                }
				return false;
			}

			if (player.altFunctionUse == 2)
			{
				item.channel = false;
				item.autoReuse = true;
				item.shoot = ModContent.ProjectileType<VarathaProj>();
				// Ensures no more than one spear can be thrown out, use this when using autoReuse
				return player.ownedProjectileCounts[item.shoot] < 1;

			}
			else
			{
				item.useTime = 20;
				item.useAnimation = 20;
				item.autoReuse = false;
				item.shoot = ModContent.ProjectileType<VarathaSwingProj>();
				item.channel = true;
			}

			return true;
		}
	}
}
