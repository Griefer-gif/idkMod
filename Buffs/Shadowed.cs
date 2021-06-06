using Idkmod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	class Shadowed : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shadowed");
			Description.SetDefault("You are surrounded by shadows");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{	
			npc.GetGlobalNPC<ModGlobalNPC>().Shadowed = true;
		}

	}
}
