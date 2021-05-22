using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
    class DarkArtsBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dark arts");
			Description.SetDefault("You are surrounded by shadows");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 10f;
			player.maxRunSpeed += 10f;
			
			player.GetModPlayer<idkPlayer>().DarkArtsBuff = true;
		}

	}
}
