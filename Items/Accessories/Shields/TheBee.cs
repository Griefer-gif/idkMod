using Idkmod;
using Idkmod.Items.Accessories.Shields;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace idkmod.Items.Accessories.Shields
{
	public class TheBee : BaseShield
	{
        readonly int MaxHealth = 100;
		public int CurrentHealth;
		private const int RechargeRate = 2;
		//public bool GotHit = true;
		public int hitTimer;

		public override bool CloneNewInstances => true;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Adds a +0.5 damage multiplier if the shield is full");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.scale = 0.5f;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Quest;
			
		}
		
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{ 
			//things i didnt find a workaround
			player.GetModPlayer<idkPlayer>().shieldsEquipped++;
			player.GetModPlayer<idkPlayer>().shieldMaxHealth = MaxHealth;

			//the function to make it recharge and a update to CurrentHealth
			ShieldRechargeUpdate(player, MaxHealth, CurrentHealth, RechargeRate, hitTimer);
			CurrentHealth = player.GetModPlayer<idkPlayer>().shieldCHealth;

			//-----------------------------
			//Shield effects here
			//-----------------------------
			if (MaxHealth >= CurrentHealth && player.GetModPlayer<idkPlayer>().shieldsEquipped == 1)
			{
				player.allDamage += 0.5f;
			}
			
		}


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var quote = new TooltipLine(mod, "", "'Float like a butterfly…'");
			quote.overrideColor = Color.Red;
			var chargeTooltip = new TooltipLine(mod, "chargeToolTip", $"shield current health: {CurrentHealth} ");
			chargeTooltip.overrideColor = Color.OrangeRed;
			tooltips.Add(new TooltipLine(mod, "", $"Shield maximum capacity: {MaxHealth} " ));
			tooltips.Add(new TooltipLine(mod, "", $"Shield recharge rate: {RechargeRate}  "));
			tooltips.Add(chargeTooltip);
			tooltips.Add(quote);
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				[nameof(CurrentHealth)] = CurrentHealth,
			};
		}

		public override void Load(TagCompound tag)
		{
			CurrentHealth = tag.GetInt(nameof(CurrentHealth));
		}

	}
}