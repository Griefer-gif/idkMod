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
				Item.NewItem(npc.getRect(), mod.ItemType("TheBee"));
				if (Main.rand.Next(7) == 0)
                {
					Item.NewItem(npc.getRect(), mod.ItemType("Hornet"));
				}
					
			}
			
		}
	}
}
