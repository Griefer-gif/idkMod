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
			Tooltip.SetDefault("'Who's a widdle gunny wunny!!! ' throws your gun every 5th shot, that gun explodes into two other guns when it hits something");
		}

		int shotNum = 0;

		public override void SetDefaults()
		{
			item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 40; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.useTime = 5; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 5; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay =5;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Green; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
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
	}
}