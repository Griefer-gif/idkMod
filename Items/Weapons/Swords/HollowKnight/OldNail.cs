
using Idkmod.Projectiles.HollowKnight;
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
		//level 1 nial
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old nail");
			Tooltip.SetDefault("A broken old nail, Its blade is blunt with age and wear.");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.useTime = 20;
			item.useAnimation = 20;
			item.reuseDelay = 60;
			item.channel = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<OldNailBlade>();
			item.shootSpeed = 10f;
		}
    }
}
