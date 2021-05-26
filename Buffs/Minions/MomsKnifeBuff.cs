using idkmod.Projectiles.Minions;
using Idkmod;
using Idkmod.Projectiles.Minions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Buffs.Minions
{
    class MomsKnifeBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mom's Knife");
			Description.SetDefault("The Knife will stab your enemies");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<idkPlayer>().PsyFlyBuff = true;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MomsKnifeMinion>()] > 0)
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
