using Idkmod;
using Idkmod.Items.Accessories.Shields;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace idkmod.Items.Accessories.Shields
{
	public class TheRoughRider : BaseShield
	{
		private const int MaxHealth = 1;
		public int CurrentHealth;
		private const int RechargeRate = 0;
		public bool GotHit = true;
		public int hitTimer = 0;
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Adds 150 health but has no shield capacity");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Cyan;

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//the function to make it recharge and a update to CurrentHealth
			ShieldRechargeUpdate(player, MaxHealth, CurrentHealth, RechargeRate, hitTimer);
			

			//------------------------------------------
			//         Shield Effects
			//------------------------------------------

			player.statLifeMax2 += 150;

			//----------------------------------------------
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var quote = new TooltipLine(mod, "", "'It takes more than that to kill a Bull Moose.'");
			quote.overrideColor = Color.Red;
			tooltips.Add(new TooltipLine(mod, "", "Shield maximum capacity: 0  "));
			tooltips.Add(new TooltipLine(mod, "", "Shield recharge rate: 0  "));
			tooltips.Add(quote);
		}


	}
}