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
using idkmod.Projectiles.ElementalBullets.ShockBullets;

namespace Idkmod.Items.Weapons.Guns.ConferenceCall
{
	class EletricityCC : ConferenceCall
	{
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int elementType;

			if (type == ProjectileID.ChlorophyteBullet)
			{
				elementType = ModContent.ProjectileType<ShockBulletHoming>();
				type = ModContent.ProjectileType<CCBulletHoming>();
			}
			else
			{
				elementType = ModContent.ProjectileType<ShockBullet>();
				type = ModContent.ProjectileType<CCBullet>();
			}

			int numberProjectiles = 8; //og = 15
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // 2 degree spread.                                                                                               
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, elementType, damage, knockBack, player.whoAmI);
			}

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var Element = new TooltipLine(mod, "", "Shock weapon");
			Element.overrideColor = Color.LightBlue;
			var ElementE = new TooltipLine(mod, "", "Shock damage slows enemies down and has a chance to paralize them");
			ElementE.overrideColor = Color.LightBlue;
			var quote = new TooltipLine(mod, "", "'Let's just ping everyone all at once.'");
			quote.overrideColor = Color.Red;
			tooltips.Add(Element);
			tooltips.Add(ElementE);
			tooltips.Add(quote);

		}

	}
}
