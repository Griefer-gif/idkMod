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
    class CoiledNail : ModItem
    {
		//level 4 nail
		public int projCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled nail");
			Tooltip.SetDefault("A strong looking nail, refined beyond all others.");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
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
			player.GetModPlayer<idkPlayer>().NailSpell2 = true;
		}
	}
}
