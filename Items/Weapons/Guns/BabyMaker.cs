using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Guns
{
	class BabyMaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BabyMaker");
			Tooltip.SetDefault("throws your gun every 5th shot, that gun explodes into two other guns when it hits something");
		}

		int shotNum = 0;

		public override void SetDefaults()
		{
			item.damage = 20; 
			item.ranged = true; 
			item.width = 40; 
			item.height = 20; 
			item.useTime = 5; 
			item.useAnimation = 5; 
			item.reuseDelay =5;
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
			if (shotNum == 4)
            {
				item.shootSpeed = 5f;
				item.reuseDelay = 30;
			}
            if (shotNum >=5)
            {
				type = ModContent.ProjectileType<idkmod.Projectiles.BBMBullet>();
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				item.shootSpeed = 16f;
				item.reuseDelay = 5;
				shotNum = 0;
			}

			shotNum++;

			return true;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var quote = new TooltipLine(mod, "", "'Who's a widdle gunny wunny!!! '");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}
	}
}