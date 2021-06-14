using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Misc.Aegis
{
	public class AegisItem : ModItem
    {
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aegis");
			Tooltip.SetDefault("The Shield of Chaos");
		}

		public override void SetDefaults()
		{
			item.damage = 48;
			item.knockBack = 15;
			item.noMelee = true;
			item.useTurn = false;
			item.ranged = true;
			item.crit = 20;
			item.rare = ItemRarityID.Pink;
			item.width = 18;
			item.height = 18;
			item.useTime = 20; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 20;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;
			item.shootSpeed = 50f;
			item.value = 10000;
			item.noUseGraphic = true;
			item.accessory = true;
		}


		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				
			}
			else
			{
				item.shoot = ModContent.ProjectileType<AegisHoldoutProj>();
				item.channel = true;//allows the hold thing
			}
			return base.CanUseItem(player);
		}
	}
}
