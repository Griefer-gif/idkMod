using idkmod.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Items
{
	public class MaquinaAgricola : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chave da Maquina Agricola");
			Tooltip.SetDefault("Maquina agricola");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 30000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = ModContent.MountType<Mounts.MaquinaAgricola>();
		}

		//public override void AddRecipes()
		//{
		//	ModRecipe recipe = new ModRecipe(mod);
		//	recipe.AddIngredient(ModContent.ItemType<ExampleItem>(), 10);
		//	recipe.AddTile(ModContent.TileType<ExampleWorkbench>());
		//	recipe.SetResult(this);
		//	recipe.AddRecipe();
		//}
	}
}