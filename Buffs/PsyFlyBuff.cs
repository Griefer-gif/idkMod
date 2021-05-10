using Idkmod;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	public class PsyFlyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Psy Fly");
			Description.SetDefault("The Psy Fly will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//BlPlayer modPlayer = player.GetModPlayer<BlPlayer>();
			//if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.PsyFlyMinion>()] > 0)
			//{
			//	modPlayer.PsyFlyEquip = true;
			//}
			//if (!modPlayer.PsyFlyEquip)	
			//{
			//	player.DelBuff(buffIndex);
			//	buffIndex--;
			//}
			//else
			//{
			//	player.buffTime[buffIndex] = 18000;
			//}
		}
	}
}