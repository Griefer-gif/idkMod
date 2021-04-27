using Idkmod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace idkmod.Items.Accessories.Shields
{
	public class TheBee : ModItem
	{
		private const int MaxHealth = 100;
		public int CurrentHealth;
		private const int RechargeRate = 2;
		public bool GotHit = true;
		public int hitTimer = 0;

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
			//Debug Stuff
			//	{
			//		Main.NewText("Shield current health :" + CurrentHealth + " ModPlayer current health :" + player.GetModPlayer<BlPlayer>().shieldCHealth);
			//		Main.NewText("gotHit shield : " + GotHit + " ModPlayer got hit : " + player.GetModPlayer<BlPlayer>().gotHit);
			//	}
			
			player.GetModPlayer<BlPlayer>().shieldsEquipped++;
			player.GetModPlayer<BlPlayer>().shieldMaxHealth += MaxHealth;
			if (GotHit == true)
			{
				int storedTime = hitTimer;
				
				int storedTime2 = player.GetModPlayer<BlPlayer>().HitTimer;

				if(storedTime != storedTime2)
                {
					hitTimer = 0;
                }

				if (hitTimer <= 600 && storedTime == storedTime2)
                {
					hitTimer++;
					player.GetModPlayer<BlPlayer>().HitTimer = hitTimer;
				}
				
				
				if (hitTimer >= 600)//600
				{
					GotHit = false;
					player.GetModPlayer<BlPlayer>().gotHit = GotHit;
					player.GetModPlayer<BlPlayer>().HitTimer = 0;
				}

				CurrentHealth = player.GetModPlayer<BlPlayer>().shieldCHealth;

			}

			
			//-----------------------------------------
			//         Shield Regen Part
			//------------------------------------------

			if (GotHit == false && CurrentHealth <= MaxHealth)
			{
				GotHit = player.GetModPlayer<BlPlayer>().gotHit;

				CurrentHealth += RechargeRate;

				player.GetModPlayer<BlPlayer>().shieldCHealth = CurrentHealth;

				if (CurrentHealth >= MaxHealth)
				{
					hitTimer = 0;
					GotHit = true;
				}
				CurrentHealth = player.GetModPlayer<BlPlayer>().shieldCHealth;
			}

			if (CurrentHealth > MaxHealth)
			{
				CurrentHealth = MaxHealth;
				
			}

			if (MaxHealth == CurrentHealth && player.GetModPlayer<BlPlayer>().shieldsEquipped == 1)
			{
				player.allDamage += 0.5f;
			}
			
			player.GetModPlayer<BlPlayer>().gotHit = GotHit;

		}
		//---------------------------------------------------------------
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var quote = new TooltipLine(mod, "", "'Float like a butterfly…'");
			quote.overrideColor = Color.Red;
			var chargeTooltip = new TooltipLine(mod, "chargeToolTip", $"shield current health: {CurrentHealth} ");
			tooltips.Add(new TooltipLine(mod, "", "Shield maximum capacity: 100  " ));
			tooltips.Add(new TooltipLine(mod, "", "Shield recharge rate: 2  "));
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