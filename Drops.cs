using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Idkmod.Items
{
    public class Drops : GlobalNPC
    {
		public override void NPCLoot(NPC npc)
		{
			if (npc.type == NPCID.KingSlime)
			{
				if (Main.rand.Next(5) == 0)
                {
					Item.NewItem(npc.getRect(), mod.ItemType("Hornet"));
				}
					
			}

			if (npc.type == NPCID.EyeofCthulhu)
			{
				if (Main.rand.Next(3) == 0)
				{
					Item.NewItem(npc.getRect(), mod.ItemType("TheBee"));
				}
			}

			if (npc.type == NPCID.QueenBee)
			{
				if (Main.rand.Next(3) == 0)
				{
					Item.NewItem(npc.getRect(), mod.ItemType("TheRoughRider"));
				}
			}

		}
	}
}
