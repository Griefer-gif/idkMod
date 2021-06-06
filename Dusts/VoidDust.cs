using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Dusts
{
	internal class VoidDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
		}

		public override bool Update(Dust dust)
		{
			//go up and go smol
			dust.scale -= 0.01f;
			
			dust.position.Y -= 1;

			// kill if too smol.
			if (dust.scale < 0.25f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}