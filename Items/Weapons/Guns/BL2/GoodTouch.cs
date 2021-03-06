using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using idkmod.Projectiles.ElementalBullets.FireBullets;

namespace Idkmod.Items.Weapons.Guns.BL2
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
			int typeElement;

			timer++;
			if(timer == 3)
            {
				player.statLife += 1;
				timer = 0;
			}

			if(type == ProjectileID.ChlorophyteBullet)
            {
				typeElement = ModContent.ProjectileType <FireBulletHoming>();
			}
            else
            {
				typeElement = ModContent.ProjectileType<FireBullet>();
			}
			
			Vector2 velocity = new Vector2(speedX, speedY);
			Projectile.NewProjectile(position, velocity, typeElement, damage, knockBack, player.whoAmI);

			return true;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var Element = new TooltipLine(mod, "", "Fire weapon");
			Element.overrideColor = Color.OrangeRed;
			var ElementE = new TooltipLine(mod, "", "Fire weapons deal more damage the more health the target has");
			ElementE.overrideColor = Color.OrangeRed;
			tooltips.Add(Element);
			tooltips.Add(ElementE);

			var quote = new TooltipLine(mod, "", "'...but when I'm bad, I'm better.'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}


	}
}