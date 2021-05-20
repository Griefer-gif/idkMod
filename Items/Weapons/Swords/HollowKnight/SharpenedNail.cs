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
	class SharpenedNail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharpened nail");
			Tooltip.SetDefault("A sharpened nail, restored to lethal form.\nAllows the use of the first level nail Spell when holding this weapon");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.useTime = 10;
			item.useAnimation = 10;
			item.reuseDelay = 30;
			item.channel = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			//item.CloneDefaults(ItemID.Arkhalis);
			item.shoot = ModContent.ProjectileType<BaseNailProj>();
			item.shootSpeed = 20f;
		}

        public override void HoldItem(Player player)
        {
			player.GetModPlayer<BlPlayer>().NailSpell1 = true;
        }
    }
}
