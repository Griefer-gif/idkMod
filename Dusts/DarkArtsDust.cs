using Terraria;
using Terraria.ModLoader;

namespace idkmod.Dusts
{
	public class DarkArtsDust : ModDust
	{

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.1f;
			dust.scale -= 0.2f;
			if (dust.scale < 0.1f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}