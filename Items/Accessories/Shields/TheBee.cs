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
	public class TheBee : BaseShield //BaseShield has all the recharge code
	{
        readonly int MaxHealth = 50;
		public int CurrentHealth;
		private const int RechargeRate = 2;
		public int hitTimer;

		public override bool CloneNewInstances => true;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Adds a 0.5 damage multiplier if the shield is full");
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
			
			//the function to make it recharge and a update to CurrentHealth
			ShieldRechargeUpdate(player, MaxHealth, CurrentHealth, RechargeRate, hitTimer);
			CurrentHealth = player.GetModPlayer<idkPlayer>().shieldCHealth;

			//-----------------------------
			//    Shield effects here
			//-----------------------------
			if (CurrentHealth >= MaxHealth)
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

		//just learned that this does not do anything, cool
		//save and load shield current health so it doesnt need to charge up everytime you enter a world
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