using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	class DarkArtsCD : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Overloaded!");
			Description.SetDefault("Cannot use Dark Arts");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BlPlayer>().DarkArtsCD = true;
		}

	}
}
