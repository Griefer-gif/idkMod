using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Buffs.TitanSouls
{
    public class SoulPowered : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul powered");
			Description.SetDefault("Your attacks are embedded with souls of the fallen");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.moveSpeed += 10f;
			//player.maxRunSpeed += 10f;

			player.GetModPlayer<idkPlayer>().SoulPowered = true;
		}
	}
}
