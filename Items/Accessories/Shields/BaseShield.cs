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

			if (player.GetModPlayer<BlPlayer>().gotHit == true && CurrentHealth != MaxHealth)
			{
				hitTimer = player.GetModPlayer<BlPlayer>().HitTimer;
				if (hitTimer <= 600)
				{
					hitTimer++;
				}


				if (hitTimer >= 600)//600
				{
					player.GetModPlayer<BlPlayer>().gotHit = false;
					hitTimer = 0;
				}

				player.GetModPlayer<BlPlayer>().HitTimer = hitTimer;
			}

			//-----------------------------------------
			//         Shield Regen Part
			//------------------------------------------

			if (player.GetModPlayer<BlPlayer>().gotHit == false && CurrentHealth < MaxHealth)
			{
				player.GetModPlayer<BlPlayer>().shieldCHealth += RechargeRate;
			}

			//
			//checking if Current health is > than max health
			//

			if (player.GetModPlayer<BlPlayer>().shieldCHealth > player.GetModPlayer<BlPlayer>().shieldMaxHealth)
			{
				player.GetModPlayer<BlPlayer>().shieldCHealth = player.GetModPlayer<BlPlayer>().shieldMaxHealth;
			}
		}

	}
}
