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

namespace Idkmod.Items.Weapons.Guns.BL2.ConferenceCall
{
    class ConferenceCall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Conference Call");
            Tooltip.SetDefault("fires 6 shots that spawns 2 other shots that hit the target again");
        }

        public override void SetDefaults()
        {
			item.damage = 50; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 40; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.useTime = 10; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 10; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 10;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item36; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f; //og:16f // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet;
			
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (type == ProjectileID.ChlorophyteBullet)
			{
				type = ModContent.ProjectileType<CCBulletHoming>();
			}
			else
			{
				type = ModContent.ProjectileType<CCBullet>();
			}

			int numberProjectiles = 8; //og = 15
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // 2 degree spread.                                                                                               
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			
			return false;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 3);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//tooltips.Add(new TooltipLine(mod, "", "Ignores ammo type"));
			var quote = new TooltipLine(mod, "", "'Let's just ping everyone all at once.'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}

	}

	
}
