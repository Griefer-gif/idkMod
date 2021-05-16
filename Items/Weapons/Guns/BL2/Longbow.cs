using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Guns.BL2
{
	class Longbow : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Longbow");
			Tooltip.SetDefault("shoots fire arrows with shit accuraccy");
		}

		public int num = 0;

		public override void SetDefaults()
		{
			item.damage = 18; 
			item.ranged = true;
			item.crit = 10;
			item.width = 20; 
			item.height = 20; 
			item.scale = 1f;
			item.useTime = 15; 
			item.useAnimation = 15; 
			item.reuseDelay = 5;
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
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3)); // 2 degree spread.
				type = ModContent.ProjectileType<idkmod.Projectiles.LongbowArrow>();
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);


			}

			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Crit damage is doubled"));
            var quote = new TooltipLine(mod, "", "'Ceci n'est pas une sniper rifle!'")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(quote);
		}
	}
}
