using idkmod.Projectiles.Minions;
using Idkmod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs.Minions
{
    class PsyFlyBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Psy Fly");
			Description.SetDefault("The Psy Fly will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BlPlayer>().PsyFlyBuff = true;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<PsyFlyMinion>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
