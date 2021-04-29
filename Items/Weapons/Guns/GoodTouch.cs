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
	class GoodTouch : ModItem
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Good Touch");
			Tooltip.SetDefault("Heals 1 hp every 3rd shot");
		}

		public override void SetDefaults()
		{
			//(60 ticks == 1 second.)
			item.damage = 10;
			item.crit = 50;
			item.ranged = true; 
			item.width = 20; 
			item.height = 20; 
			item.scale = 1f;
			item.useTime = 20; 
			item.useAnimation = 0; 
			item.reuseDelay = 10;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; 
			item.knockBack = 4;
			item.value = 10000; 
			item.rare = ItemRarityID.Quest; 
			item.UseSound = SoundID.Item11; 
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 50f; 
			item.useAmmo = AmmoID.Bullet; 

		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}

        public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            base.PickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			timer++;
			if(timer == 3)
            {
				player.statLife += 1;
				timer = 0;
			}

			type = ModContent.ProjectileType<idkmod.Projectiles.FireBullet>();
			Vector2 velocity = new Vector2(speedX, speedY);
			Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);

			return false;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Fires fire bullets"));

			var quote = new TooltipLine(mod, "", "'...but when I'm bad, I'm better.'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}


	}
}