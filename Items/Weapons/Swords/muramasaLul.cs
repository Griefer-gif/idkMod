using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Swords
{
	public class muramasaLul : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Assasin's sword"); 
			Tooltip.SetDefault("The sword once wielded by gods");
		}

		public override void SetDefaults() 
		{
			item.damage = 15;
			item.melee = true;
			item.Size = new Vector2(38);
			item.scale = 1.4f;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 10;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.Diamond, 1);
            
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}