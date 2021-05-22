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
	public abstract class BaseShield : ModItem
	{
		public void ShieldRechargeUpdate(Player player, int MaxHealth, int CurrentHealth, int RechargeRate, int hitTimer)
		{

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

	}
}
