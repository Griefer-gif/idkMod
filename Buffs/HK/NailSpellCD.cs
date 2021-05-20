using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Buffs.HK
{
	class NailSpellCD : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Recharging soul!");
			Description.SetDefault("Cannot use Nail Spells");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BlPlayer>().NailSpellCD = true;
		}

	}
}
