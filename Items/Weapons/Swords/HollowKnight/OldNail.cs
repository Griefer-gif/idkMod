using idkmod.Projectiles.HollowKnight;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Swords.HollowKnight
{
    class OldNail : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old nail");
			Tooltip.SetDefault("Just a rusty old nail");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.melee = false;
			item.width = 20;
			item.height = 12;
			item.useTime = 10;
			item.useAnimation = 10;
			item.reuseDelay = 5;
			//item.channel = true;
			//item.noUseGraphic = true;
			//item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6;
			item.value = Item.buyPrice(0, 22, 50, 0);
			item.rare = ItemRarityID.White;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<OldNailProj>();
			item.shootSpeed = 40f;
		}
	}
}
