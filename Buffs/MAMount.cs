using Terraria;
using Terraria.ModLoader;

namespace idkmod.Buffs
{
	public class MAMount : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Maquina Agricola");
			Description.SetDefault("poderosa");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Mounts.MaquinaAgricola>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}