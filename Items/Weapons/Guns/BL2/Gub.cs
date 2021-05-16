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
using idkmod.Projectiles.ElementalBullets.FireBullets;
using idkmod.Projectiles.ElementalBullets.CorrosiveBullets;

namespace Idkmod.Items.Weapons.Guns.BL2
{
    class Gub : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Gub");
			Tooltip.SetDefault("shoots slow speed bullets");
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
			item.shootSpeed = 1f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int typeElement;

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			if (type == ProjectileID.ChlorophyteBullet)
			{
				typeElement = ModContent.ProjectileType<CorrosiveBulletHoming>();
			}
			else
			{
				typeElement = ModContent.ProjectileType<CorrosiveBullet>();
			}

			Vector2 velocity = new Vector2(speedX, speedY);
			Projectile.NewProjectile(position, velocity, typeElement, damage, knockBack, player.whoAmI);

			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Converted bullets have a very slow speed"));
            var Element = new TooltipLine(mod, "", "Corrosive weapon")
            {
                overrideColor = Color.Green
            };
            var ElementE = new TooltipLine(mod, "", "Corrosion damage scales with low enemy health")
            {
                overrideColor = Color.Green
            };
            var quote = new TooltipLine(mod, "", "'Abt Natural'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(Element);
			tooltips.Add(ElementE);
			tooltips.Add(quote);
		}
	}
}
