
using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	//Electric debuff
	public class Electrocuted : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Electrocuted");
			Description.SetDefault("You are being shocked by electricity");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<idkPlayer>().Shock = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ModGlobalNPC>().Shock = true;
		}
	}
}