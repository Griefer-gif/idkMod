using Idkmod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace Idkmod.Items.Accessories.Shields
{
	//Exclusivity code was taken from ExampleMod, ty direWolf
	public abstract class BaseShield : ModItem
	{
		//Exclusivity code
		public override bool CanEquipAccessory(Player player, int slot)
		{
			if (slot < 10)
			{
				int index = FindDifferentEquippedExclusiveAccessory().index;
				if (index != -1)
				{
					return slot == index;
				}
			}

			return base.CanEquipAccessory(player, slot);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			Item accessory = FindDifferentEquippedExclusiveAccessory().accessory;
			if (accessory != null)
			{
				tooltips.Add(new TooltipLine(mod, "Swap", "Right click to swap with '" + accessory.Name + "'!")
				{
					overrideColor = Color.OrangeRed
				});
			}
		}

		public override void RightClick(Player player)
		{
			var (index, accessory) = FindDifferentEquippedExclusiveAccessory();
			if (accessory != null)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(accessory);

				Main.LocalPlayer.armor[index] = item.Clone();
			}
		}

		//-------------------------------------------------------
		public void ShieldRechargeUpdate(Player player, int MaxHealth, int CurrentHealth, int RechargeRate, int hitTimer)
		{
			player.GetModPlayer<idkPlayer>().shieldMaxHealth = MaxHealth;
			if (player.GetModPlayer<idkPlayer>().gotHit == true && CurrentHealth != MaxHealth)
			{
				hitTimer = player.GetModPlayer<idkPlayer>().HitTimer;
				if (hitTimer <= 600)
				{
					hitTimer++;
				}


				if (hitTimer >= 600)//600
				{
					player.GetModPlayer<idkPlayer>().gotHit = false;
					hitTimer = 0;
				}

				player.GetModPlayer<idkPlayer>().HitTimer = hitTimer;
			}

			//-----------------------------------------
			//         Shield Regen Part
			//------------------------------------------

			if (player.GetModPlayer<idkPlayer>().gotHit == false && CurrentHealth < MaxHealth)
			{
				player.GetModPlayer<idkPlayer>().shieldCHealth += RechargeRate;
			}

			//
			//checking if Current health is > than max health
			//

			if (player.GetModPlayer<idkPlayer>().shieldCHealth > player.GetModPlayer<idkPlayer>().shieldMaxHealth)
			{
				player.GetModPlayer<idkPlayer>().shieldCHealth = player.GetModPlayer<idkPlayer>().shieldMaxHealth;
			}
		}

		protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 3; i < 3 + maxAccessoryIndex; i++)
			{
				Item otherAccessory = Main.LocalPlayer.armor[i];
				
				if (!otherAccessory.IsAir &&
					!item.IsTheSameAs(otherAccessory) &&
					otherAccessory.modItem is BaseShield)
				{
					return (i, otherAccessory);
				}
			}
			
			return (-1, null);
		}

	}
}
