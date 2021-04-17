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
	class Infinity : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infinity");
			Tooltip.SetDefault("'It's closer than you think! (no it isn't)', Does not consume ammo, Converts Musket balls into high speed bullets");
		}

		public int num = 0;
		public bool switc = true;

		public override void SetDefaults()
		{
			item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 0.8f;
			item.useTime = 5; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 30; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 0;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Green; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
		}

		public override bool ConsumeAmmo(Player player)
		{
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Vector2 perturbedSpeed = new Vector2(speedX, speedY);
			Vector2 perturbedSpeed2 = new Vector2(speedX, speedY);

			if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
			{
				type = ProjectileID.BulletHighVelocity; // or ProjectileID.FireArrow;
			}

			//Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			//if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			//{
			//	position += muzzleOffset;
			//}
			
			if (num <= -3)
			{
				switc = true;
			}

			if (num >= 3)
            {
				switc = false;
            }

			if (switc == true)
            {
				speedY = perturbedSpeed.Y += num;
				perturbedSpeed2.Y -= num;
				num += 1;
            }
            else
            {
				speedY = perturbedSpeed.Y += num;
				perturbedSpeed2.Y -= num;
				num -= 1;
			}

			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, knockBack, player.whoAmI);

			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}
	}
}