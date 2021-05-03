
using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	//Electric debuff
	public class Burning : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Burning!");
			Description.SetDefault("You are being burned alive by fire");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BlPlayer>().Fire = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ModGlobalNPC>().Fire = true;
		}
	}
}