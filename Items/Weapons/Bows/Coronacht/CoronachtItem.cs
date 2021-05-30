using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Idkmod.Projectiles.Hades;

namespace Idkmod.Items.Weapons.Bows.Coronacht
{
    public class CoronachtItem : ModItem
    {
		//spirit mod's cornucopion was used as reference, ty
		//Notes: left click needs to be used once before right click works, idk why this happnes, needs fix
		//there was a index out of bounds error while testing the bow, only happened once, but it might be a problem 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coronacht");
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
			item.useAmmo = AmmoID.Arrow;
			item.shootSpeed = 50f;
			item.value = 10000;
			item.noUseGraphic = false;
		}


		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}


		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(player.altFunctionUse == 2)
            {
				float numberProjectiles = 7; // 3, 4, or 5 shots
				float rotation = MathHelper.ToRadians(30);
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CoronachtArrow>(), damage, knockBack, player.whoAmI);
				}
				return false;
			}
            
			return true;
        }

        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useAmmo = AmmoID.Arrow;
			}
			else
			{
				item.useAmmo = AmmoID.None;
				item.shoot = ModContent.ProjectileType<CoronachtProj>();
				item.channel = true;//allows the hold thing
			}
			return base.CanUseItem(player);
		}
	}
}
