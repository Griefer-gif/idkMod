using Idkmod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Items.Accessories
{
	//
	//Default thingies for all shields
	//
    public abstract class Shields : ModItem
	{
		public void SetShieldValues(Player player, float MaxHealt,float RRate)
		{
			player.GetModPlayer<BlPlayer>().shieldMaxHealth = MaxHealt;
			player.GetModPlayer<BlPlayer>().shieldRRate = RRate;
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 32;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SunStone, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			if (slot < 10) 
			{
				int index = FindDifferentEquippedExclusiveAccessory().index;
				if (index != -1)
				{
					return slot == index;
				}
			}
		
			return base.CanEquipAccessory(player, slot);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Item accessory = FindDifferentEquippedExclusiveAccessory().accessory;
			if (accessory != null)
			{
				tooltips.Add(new TooltipLine(mod, "Swap", "Right click to swap with '" + accessory.Name + "'!")
				{
					overrideColor = Color.OrangeRed
				});
			}
		}

		public override bool CanRightClick()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 13; i < 13 + maxAccessoryIndex; i++)
			{
				if (Main.LocalPlayer.armor[i].type == item.type) return false;
			}

			if (FindDifferentEquippedExclusiveAccessory().accessory != null)
			{
				return true;
			}

			return base.CanRightClick();
		}

		public override void RightClick(Player player)
		{
			var (index, accessory) = FindDifferentEquippedExclusiveAccessory();
			if (accessory != null)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(accessory);
				// We need to use index instead of accessory because we directly want to alter the equipped accessory
				Main.LocalPlayer.armor[index] = item.Clone();
			}
		}



		protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 3; i < 3 + maxAccessoryIndex; i++)
			{
				Item otherAccessory = Main.LocalPlayer.armor[i];

				if (!otherAccessory.IsAir &&
					!item.IsTheSameAs(otherAccessory) &&
					otherAccessory.modItem is Shields)
				{

					return (i, otherAccessory);

				}
			}

			return (-1, null);
		}
	}

	//
	// Here we add our accessories, note that they inherit from ExclusiveAccessory, and not ModItem
	//

	//
	// THE BEE	
	//
	public class TheBee : Shields
	{ 
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("If shield Health is full, adds 50 damage to all attacks");
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SetShieldValues(player, 50, 5);

			player.GetModPlayer<BlPlayer>().shieldCHealth += 50;


			if (player.GetModPlayer<BlPlayer>().shieldCHealth == player.GetModPlayer<BlPlayer>().shieldMaxHealth)
            {
				player.rangedDamage += 5;
			}
		}

		public override void RightClick(Player player)
		{
			string previousItemName = "";

			Item accessory = FindDifferentEquippedExclusiveAccessory().accessory;
			if (accessory != null)
			{
				previousItemName = accessory.Name;
			}

			base.RightClick(player);

			Main.PlaySound(SoundID.MaxMana, (int)player.position.X, (int)player.position.Y);
		}
	}

	//
	//Shit shield
	//

	public class YellowExclusiveAccessory : Shields
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 100% at day, and ranged damage at night");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = ItemRarityID.Yellow;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SunStone, 1);
			recipe.AddIngredient(ItemID.MoonStone, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			//recipe.AddRecipe();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.dayTime)
			{
				player.meleeDamage += 1f;
			}
			else
			{
				player.rangedDamage += 1f;
			}
		}

		public override void RightClick(Player player)
		{
			string previousItemName = "";

			Item accessory = FindDifferentEquippedExclusiveAccessory().accessory;
			if (accessory != null)
			{
				previousItemName = accessory.Name;
			}

			base.RightClick(player);

			Main.PlaySound(SoundID.MaxMana, (int)player.position.X, (int)player.position.Y);
		}
	}

	//
	//DEBUG SHIELD
	//

	public class TestShield : Shields
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 100% at day, and ranged damage at night");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = ItemRarityID.Yellow;
		}

		public override void AddRecipes()
		{
	
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SunStone, 1);
			recipe.AddIngredient(ItemID.MoonStone, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			//recipe.AddRecipe();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.dayTime)
			{
				
				player.meleeDamage += 1f;
			}
			else
			{
				
				player.rangedDamage += 1f;
			}
		}

		public override void RightClick(Player player)
		{
			string previousItemName = "";

			Item accessory = FindDifferentEquippedExclusiveAccessory().accessory;
			if (accessory != null)
			{
				previousItemName = accessory.Name;
			}

			base.RightClick(player);

			Main.PlaySound(SoundID.MaxMana, (int)player.position.X, (int)player.position.Y);
		}
	}

	
}