using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Bows.TitanSouls
{
    public class TitanSlayer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titan Slayer");
			Tooltip.SetDefault("The heart-seeking bow");
		}

		public override void SetDefaults()
		{
			item.damage = 48;
			item.knockBack = 15;
			item.noMelee = true;
			item.useTurn = false;
			item.ranged = true;
			item.crit = 20;
			item.rare = ItemRarityID.Pink;
			item.width = 18;
			item.height = 18;
			item.useTime = 20; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 20;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;
			item.useAmmo = AmmoID.None;
			item.channel = true;
			item.shootSpeed = 50f;
			item.shoot = ModContent.ProjectileType<TitanSlayerHoldout>();
			item.value = 10000;
			item.noUseGraphic = false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}

        public override bool CanUseItem(Player player)
        {
			//Main.NewText("yep");
            for(int i = 0; Main.projectile.Length > i; i++)
            {
				if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TitanSlayerArrow>() && Main.projectile[i].active)
                {
					item.shoot = ModContent.ProjectileType<ArrowMagnet>();
					item.noUseGraphic = true;
					return true;
				}
			}
			item.shoot = ModContent.ProjectileType<TitanSlayerHoldout>();
			item.noUseGraphic = false;
			return true;
        }
    }
}
