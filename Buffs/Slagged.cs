
using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	//slagged, i mean, not too complicated
	public class Slagged : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slagged");
			Description.SetDefault("You are more vulnerable to elemental damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<idkPlayer>().Slagg = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ModGlobalNPC>().Slagg = true;
		}
	}
}