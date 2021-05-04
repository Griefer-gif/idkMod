using Idkmod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace idkmod.Items.Accessories.Shields
{
	public class TheRoughRider : ModItem
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
			//Debug Stuff
			//	{
			//Main.NewText("Shield current health :" + CurrentHealth + " ModPlayer current health :" + player.GetModPlayer<BlPlayer>().shieldCHealth);
			//Main.NewText("gotHit shield : " + GotHit + " ModPlayer got hit : " + player.GetModPlayer<BlPlayer>().gotHit);
			//	}

			player.GetModPlayer<BlPlayer>().shieldsEquipped++;
			player.GetModPlayer<BlPlayer>().shieldMaxHealth += MaxHealth;
			if (GotHit == true)
			{
				int storedTime = hitTimer;

				int storedTime2 = player.GetModPlayer<BlPlayer>().HitTimer;

				if (storedTime != storedTime2)
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


			//------------------------------------------
			//         Shield Regen Part(commented out because rr does not have regen)
			//------------------------------------------

			player.GetModPlayer<BlPlayer>().shieldCHealth = CurrentHealth;

			//if (GotHit == false && CurrentHealth <= MaxHealth)
			//{
			//	GotHit = player.GetModPlayer<BlPlayer>().gotHit;
			//
			//	player.GetModPlayer<BlPlayer>().shieldCHealth = CurrentHealth;
			//
			//	CurrentHealth += RechargeRate;
			//
			//	if (CurrentHealth >= MaxHealth + 5)
			//	{
			//		hitTimer = 0;
			//		GotHit = true;
			//	}
			//}

			if (CurrentHealth >= MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}

			//------------------------------------------
			//         Shield Effects
			//------------------------------------------

			player.statLifeMax2 += 150;

			//----------------------------------------------

			player.GetModPlayer<BlPlayer>().gotHit = GotHit;
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