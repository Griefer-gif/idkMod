using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles;

namespace Idkmod.Items.Weapons.Guns
{
    class Gub : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Gub");
			Tooltip.SetDefault("Converts musket balls into corossive bullets");
		}

        public override void SetDefaults()
        {
			item.damage = 30;
			item.ranged = true;
			item.width = 20;
			item.height = 20;
			item.scale = 0.8f;
			item.useTime = 3;
			item.useAnimation = 3;
			item.reuseDelay = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = ItemRarityID.Quest;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			if (type == ProjectileID.Bullet)
			{
				for (int i = 0; i < 10; i++)
                {
					Dust.NewDust(position, 5, 5, DustID.Flare, speedX, speedY, 0, Color.Green, 1.1f);
				}

				item.shootSpeed = 0.1f;

				type = ProjectileID.CursedBullet;
			}
			else
            {
				item.shootSpeed = 16f;
			}
			
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Converted bullets have a very slow speed"));
			var quote = new TooltipLine(mod, "", "'Abt Natural'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}
	}
}
