
using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	// Corrosive debuff
	public class Melting : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Melting");
			Description.SetDefault("You are melting with corrosion");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BlPlayer>().Corrosive = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ModGlobalNPC>().Corrosive = true;
		}
	}
}