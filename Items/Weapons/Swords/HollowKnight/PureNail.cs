using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Idkmod.Projectiles.HollowKnight;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Swords.HollowKnight
{
	class PureNail : ModItem
	{
		//level 5 nail
		public int projCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pure nail");
			Tooltip.SetDefault("A perfect looking nail, crafted to perfection, this ancient nail reveals its true form.");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.useTime = 5;
			item.useAnimation = 5;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<UpgradedNailBlade>();
			item.shootSpeed = 40f;
		}

		public override void HoldItem(Player player)
		{
			player.GetModPlayer<idkPlayer>().NailSpell1UP = true;
			player.GetModPlayer<idkPlayer>().NailSpell2UP = true;
		}
	}
}
